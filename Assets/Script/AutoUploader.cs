#if UNITY_EDITOR
using NKStudio.Discord;
using NKStudio.Discord.Module;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[InitializeOnLoad]
public class AutoUploader
{
    // This function is automatically called when a build is completed.
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        UnityEngine.Debug.Log("빌드 완료! 구글 드라이브 업로드를 시작합니다.");

        // Get the path of the built folder.
        string buildDirectoryPath = Path.GetDirectoryName(pathToBuiltProject);
        string buildFolderName = Path.GetFileNameWithoutExtension(pathToBuiltProject);

        // Define the path for the new ZIP file, placing it one level above the build folder.
        string zipFileName = $"{Application.productName}_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
        string zipFilePath = Path.Combine(Path.GetDirectoryName(buildDirectoryPath), zipFileName);

        // Compress the build folder into a ZIP file.
        try
        {
            // Add a short delay to ensure files are not in use.
            Thread.Sleep(2000); // 2-second delay

            UnityEngine.Debug.Log($"빌드 폴더를 압축하는 중: {buildDirectoryPath} -> {zipFilePath}");
            ZipFile.CreateFromDirectory(buildDirectoryPath, zipFilePath);
            UnityEngine.Debug.Log("압축 완료!");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"압축 중 오류 발생: {e.Message}");
            return;
        }

        // Use rclone to upload the compressed ZIP file to Google Drive.
        string rcloneRemoteName = "rclone으로 만든 이름적어주세요"; // The name of the remote set in rclone config
        string rcloneDestPath = " 구글드라이브 폴더명을 입력하세요"; // The destination folder in Google Drive

        string arguments = $"copy \"{zipFilePath}\" \"{rcloneRemoteName}:{rcloneDestPath}\"";

        // Specify the full path of the rclone executable.
        // The path uses the Unity project's internal folder.
        string rclonePath = Path.Combine(Application.dataPath, @"BuildAutoUpload/rclone-v1.71.0-windows-amd64/rclone.exe");

        if (!File.Exists(rclonePath))
        {
            UnityEngine.Debug.LogError($"rclone.exe 파일을 지정된 경로에서 찾을 수 없습니다: {rclonePath}. 경로를 확인해주세요.");
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = rclonePath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true, // Redirect error output
            CreateNoWindow = true
        };

        try
        {
            Process process = Process.Start(startInfo);
            // Wait until the rclone process finishes, then read the output.
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (process.ExitCode == 0)
            {
                UnityEngine.Debug.Log("구글 드라이브 업로드 성공!");
                DiscordBot.Create("디스코드웹후크 봇 생성후 토큰등록")
                .WithEmbed
                    (
                    Embed.Create().WithURL("구글드라이브링크(선택사항 필요없으니 지워도됨)").WithTitle(zipFileName).WithDescription("빌드가 성공적으로 완료되었습니다.").WithColor(Color.green)
                    ).Send();

                // Delete the ZIP file after successful upload.
                File.Delete(zipFilePath);
                UnityEngine.Debug.Log("업로드된 ZIP 파일 삭제 완료!");
            }
            else
            {
                UnityEngine.Debug.LogError($"구글 드라이브 업로드 실패: rclone 종료 코드 {process.ExitCode}");
                UnityEngine.Debug.LogError($"rclone 출력: {output}");
                UnityEngine.Debug.LogError($"rclone 에러: {error}");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"업로드 중 오류 발생: {e.Message}");
        }
    }
}
#endif
