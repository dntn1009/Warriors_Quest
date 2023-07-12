# Warriors_Quest
 
2023-07-06
1. Terrain 맵 구현
2. asset 자료 수집
3. 구현에 필요한 자료 수집

2023-07-07
1. 플레이어 object 생성
   -object animator 및 animation 구현
   -움직임 구현 및 character controller 및 playercontroller.cs 생성 및 구현
   -player의 장비 착용 유무를 이용하기 위해 초기 설정 구현
2. 플레이어의 움직임을 위해 asset 자료 수집
3. MainCamera의 player 추적 및 회전값 적용 구현 70%
   -각각 추적하거나 회전하기는 되지만 두 함수 적용 시 추적만 적용됨 수정이 필요함.

2023-07-08 ~ 2023-07-09 개인사정

2023-07-10
1.플레이어 Object 움직임 및 카메라 회전 구현 완료
2.Terrain Tree & grass 제거 후 새로 구현 진행 중.

2023-07-11
1. 카메라가 보는 forward(정면)에 맞춰 캐릭터Object 안에 있는 CharacterController의 SimpleMove가 움직일수 있도록 Vector3의 값을 변경시켜주려고 함.
   - camera의 forward(y = 0)을 이용하여 움직임에 추가를 줄 예정 70% 구현
2. object들의 크기를 맞추고 Terrain의 Tree 재배열 및 텍스쳐 수정
