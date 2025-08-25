Unity 빌드 & rclone이용한 자동 업로드 시스템 + 디스코드 알림 시스템
이 프로젝트는 Unity 빌드가 완료되면 **rclone**을 사용해 빌드 파일을 Google Drive에 자동으로 업로드하는 기능을 포함하고 있습니다. 팀원들과의 협업 및 빌드 버전 관리에 매우 유용합니다.

🚀 시작하기 전 준비물
이 시스템을 사용하려면 다음 세 가지가 필요합니다.

Unity Editor: 프로젝트를 빌드하기 위한 유니티 에디터

rclone (v1.71.0-windows-amd64): 파일 동기화 및 업로드를 위한 프로그램

Google Drive 계정: 빌드 파일을 저장할 클라우드 공간

⚙️ 3단계 설정 방법
1. rclone 설정하기
rclone을 처음 사용한다면 rclone config 명령어를 실행하여 Google Drive와 연결해야 합니다.

원격 저장소 이름은 스크립트와 동일한 **meami(예시)**로 설정해야 합니다.

자세한 설정 방법은 rclone 공식 문서를 참고하세요.

2. 프로젝트에 파일 배치하기
rclone 실행 파일과 자동 업로드 스크립트를 프로젝트에 복사해 넣습니다.

다운로드한 rclone-v1.71.0-windows-amd64 폴더를 Unity 프로젝트 내 Assets/rclone 경로에 복사하세요.

AutoUploader.cs 스크립트는 Assets/Editor 폴더에 넣습니다. (만약 폴더가 없으면 직접 생성해주세요.)

3. 스크립트 경로 확인하기
AutoUploader.cs 스크립트를 열어서 rclone 실행 파일의 경로가 올바르게 설정되었는지 확인하세요.

string rclonePath = Path.Combine(Application.dataPath, @"rclone/rclone-v1.71.0-windows-amd64/rclone.exe");
```rclone` 폴더의 이름이나 위치를 다르게 지정했다면, 위 코드의 경로를 프로젝트에 맞게 수정해야 합니다.

---

### (선택사항)
https://nk-studio.github.io/Packages/com.nkstudio.udiscordbot@1.0/manual/index.html
유니티에서 디스코드에 접근할수있는 플러그인
https://www.youtube.com/watch?v=U3bD3aOga1c
적용 예시 영상

---

### ✅ 사용 방법

모든 설정이 완료되었다면, 이제 빌드를 누르는 것만으로도 모든 과정이 자동으로 진행됩니다.

1.  Unity 에디터에서 `File` > `Build Settings`로 이동합니다.
2.  빌드 옵션을 확인하고 `Build` 버튼을 클릭합니다.
3.  빌드가 끝나면 **빌드된 폴더 전체**가 Google Drive에 자동으로 업로드됩니다.

빌드 및 업로드 진행 상황은 유니티 **Console** 창에서 실시간으로 확인할 수 있습니다.

---

### 프로젝트를 다운로드해서 채험해볼시

디스코드 (빌드알림 ) https://discord.gg/9BypKE2KGc
구글드라이브 업로드되는 폴더 https://drive.google.com/drive/folders/1WaaRLNnnMTOT0L6i68ybcK2Gc20fkyre?usp=sharing

