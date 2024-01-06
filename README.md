# KLTN_EvergreenGarden
Final project for university graduation - Farm game built with Unity - Pro version
# Set up
## Import package:
- GoogleSignIn Plugin 1.0.4
- FirebaseAuth (new version - 28/08/2023)
- Facebook SDK for Unity - link: https://developers.facebook.com/docs/unity/
## Import asset:
- DOTween 1.2.745
- Happy Harvest 1.01
## Video, Image demo + Link CH Play
- Link video: https://www.youtube.com/watch?v=kV_mHByXnhg | https://www.youtube.com/watch?v=2aTGiB7V1fw
- Link CH Play: Loading...

| **STT** | **Scene** | **Demo** |
| :------: | :------: | :------: |
| 1 | MenuScene | <img alt="MenuScene" src="https://private-user-images.githubusercontent.com/89081979/294146520-e319e78b-8262-4d3e-8bb0-c7dbb3ac9e3d.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3MDQ1MjQwOTgsIm5iZiI6MTcwNDUyMzc5OCwicGF0aCI6Ii84OTA4MTk3OS8yOTQxNDY1MjAtZTMxOWU3OGItODI2Mi00ZDNlLThiYjAtYzdkYmIzYWM5ZTNkLmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNDAxMDYlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjQwMTA2VDA2NDk1OFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWQ0MDFhMDQ0NzZiMDcyMjFmYjExYjE0ZTdhOWY2NzA4N2QwMGZiMGE5Mzc0NWQ4OGVmMjM3N2NhOTRkMTkzZDImWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0JmFjdG9yX2lkPTAma2V5X2lkPTAmcmVwb19pZD0wIn0.-2FMwbEIaVpo_tgQrmAYmGkEXabWTIBFok34bu3R8zg" width="600"> |
| 2 | PlayScene | <img alt="PlayScene" src="https://private-user-images.githubusercontent.com/89081979/294144253-bbeb06b2-80ab-4730-a5c2-2cacc2bcfcb2.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3MDQ1MjQwOTgsIm5iZiI6MTcwNDUyMzc5OCwicGF0aCI6Ii84OTA4MTk3OS8yOTQxNDQyNTMtYmJlYjA2YjItODBhYi00NzMwLWE1YzItMmNhY2MyYmNmY2IyLmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNDAxMDYlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjQwMTA2VDA2NDk1OFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWUzZTY2NDI4OWFmNjJkOTVjYTg1NDU5M2NjODI3NmZlMTcxNzA1MDIzOTMwNjI5MTcwMjY0MWNlODIyNDNiNjImWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0JmFjdG9yX2lkPTAma2V5X2lkPTAmcmVwb19pZD0wIn0.gi6JnrOY_2bfEvndJioMM5a4h17R8drap8l0ZvLs6Cg" width="600"> |
| 3 | FriendScene | <img alt="FriendScene" src="https://private-user-images.githubusercontent.com/89081979/294146536-96ac84fc-ddcc-47cd-b63f-fd260c350863.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3MDQ1MjQwOTgsIm5iZiI6MTcwNDUyMzc5OCwicGF0aCI6Ii84OTA4MTk3OS8yOTQxNDY1MzYtOTZhYzg0ZmMtZGRjYy00N2NkLWI2M2YtZmQyNjBjMzUwODYzLmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNDAxMDYlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjQwMTA2VDA2NDk1OFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTI2ZmZlZDhhMjc0YTQ4MTdjNzc3OGIzNjFiODQwZGY1YzY1Nzk4MDExMzVlMjE1MTNiYWNiYmE4ZDIxZmU3MTUmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0JmFjdG9yX2lkPTAma2V5X2lkPTAmcmVwb19pZD0wIn0.Ygi-i-ClwqPZsV_DnJYzPhg59D4Dm3EI4LdIMrC58VI" width="600"> |
| 4 | ChatScene | <img alt="ChatScene" src="https://private-user-images.githubusercontent.com/89081979/294146606-41ef28ca-aab0-4431-aa67-553816631ec9.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3MDQ1MjQwOTgsIm5iZiI6MTcwNDUyMzc5OCwicGF0aCI6Ii84OTA4MTk3OS8yOTQxNDY2MDYtNDFlZjI4Y2EtYWFiMC00NDMxLWFhNjctNTUzODE2NjMxZWM5LmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNDAxMDYlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjQwMTA2VDA2NDk1OFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTU2NDI3MjJmNzBjYjZlZGFhZmQwNDdiZWIyOTllZDk3NzYxM2U2ZGI3N2JmNmE5MDg2NGYxMTQ1MjhkMTExOGQmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0JmFjdG9yX2lkPTAma2V5X2lkPTAmcmVwb19pZD0wIn0.MrOJH7-n0GvG8N5ybWVFDqacWZwy8ncA_Wd1Y3gtuCM" width="600"> |
