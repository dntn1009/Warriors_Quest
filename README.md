# Warriors_Quest

캐릭터 기준으로 어떤 시점을 이용할 지 선택지가 두가지
1. Top View 형식 & Nav Agent를 이용하여 해당 클릭의 상황에 맞춰 반응
2. GetAxis를 이용하여 키보드로 움직이면서 카메라를 회전하여 움직이도록 구현 (카메라의 Forward의 방향에 맞춰 움직이도록 구현)
2번으로 구현하도록 기획.

카메라 회전 시 선택지 두가지
1. CentralAxis라는 Object를 생성, 그안에 카메라와 Player를 자식 오브젝트로 삼아, 카메라 각도를 조절하고 거기에 맞춰 PlayerObject가 대응
2. Player를 카메라가 추적및 회전하도록 PlayerController.Script에 구현

인게임 상황 시에 Player를 불러오거나 직업이 전사, 마법사 등 다양해 질 경우 2번이 더 적합하다고 판단하여 2번으로 기획.
 
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

2023-07-12
1. Player Object & Camera를 연동하여 카메라의 앞에 맞춰 움직이도록 구현(2023-07-11 camera의 forward(y = 0)) 완료함.
   - Camera의 forward & Right를 이용하여 카메라의 앞부분을 플레이어의 앞부분으로 변경하도록 한다.
     카메라의 forward대로 움직이기 위해 GetAxis의 값을 Camera의 forward& Right를 곱하거나 더하여 움직이도록 구현함.
2. Player 무기 장착 Idle & 기본 Idle 구현 완료함.
3. Attack 기초 코딩 구현함.
4. Weapon Run & Attack Animation 부자연스러운 모습 확인 수정이 필요함.

2023-07-13 ~ 2023-07-14
1. Player의 움직임에 많은 부자연스러운 부분을 발견하여 확인하였더니 FBX의 버전에 따라 움직임이 부자연스러울 수 있다는 점을 발견함.
   - MixAmo 사이트에 현재 사용하고 있던 Character fbx를 업로드하여 다운로드하여 fbx 버전을 맞춤.
     캐릭터의 컬러 및 디자인 원래대로 구현
2. Player의 Animation 중 공격 애니메이션 경우 PlayerObject가 회전하거나 움직이는 경우가 발생하여 확인
   - Animator의 RootMotion을 건드리면 공중 모션 & 움직이는 모션 등이 땅에서 움직여 매우 부자연스러운 현상이 발생
     Inspector에서 Animation을 관련하는 부분에 Bake Into Pose를 체크하여 움직이지 않고 애니메이션만 정상적으로 나오도록 구현 완료.\
3. Player Attack시 필요한 Collider & Script & 자식 Object 생성 (이 과정에서 필수로 필요한 Tag와 Layer 생성)
   -Object에 자식 오브젝트인 AttackArea 생성 그안에 있는 자식 오브젝트들(= Area1, 2, 3 ''')이 Attack들과 반응하도록 구현할 예정
    자식오브젝트인 Area에 Collider 및 AttackAreUnitFind Scirpt를 추가함, Collider가 Monster에 반응하는지 확인함.
    AttackAreUnitFind는 Area의 Collider별로 반응하게 하기위해 만든 Script


