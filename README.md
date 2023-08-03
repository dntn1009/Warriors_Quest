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

2023-07-17 ~ 2023-07-18
1. AttackAreUnitFind.Script에서 List UnitList에 Collider에 OntriggerEnter하면 Add Exit하면 Remove 하여 Unit List를 받은 후
   PlayerController에서 AttackAreUnitFind의 배열체를 가져와서 공격 타이밍에 읽어온 UnitList에 데미지를 주는 방법
   BoxCollider를 끌 이유가 없음.
2. AttackAreUnitFInd.Script에서 PlayerController를 함수로 등록하는 Initialize 함수 생성 후 Collider의 OntriggerEnter에 들어오면
   PlayerController.script의 Attack()함수가 적용 되도록 함. PlayerController.Script에서는 BoxCollider의 배열체를 가져온 후.
   처음 세팅으로 BoxCollider를 끄고 각 배열체안에 있는 AttackAreUnitFind에서 본인의 Controller.Script를 가져가도록 InitializeSet()함수를 적용시킴.
   그 후, BoxCollider를 끄고 키고 하며 데미지를 주도록 함.

   1번 방법 사용시 (동적콜라이더)RIgidbody Iskinematic을 사용하여 Cpu부하를 막아야 함. 계속 킨상태로 움직이면 메모리와 성능면에서 우위를 점할지가 고민됨.
   2번 방법 사용시 (정적콜라이더)Rigidbody를 사용 안해도 됨. 어택 시에 콜라이더를 활성화 하기 때문에 메모리와 성능면에서 우위를 점할거 같음.

   2번 방법을 이용하여 구현하기로 함.
   + Attack 애니메이션 이용 시 적절하고 매끄러운 연결을 위해서 PlayerController의 스크립트를 재정리 및 코딩 추가 구현을 할 필요가 있음
   + Monster를 구현하기 위한 Object 찾기 Boss & Normal 몬스터들을 구현할 Object들 정리 및 찾는 중.

2023-07-19 ~ 2023-07-20
1. 일반 몬스터들 & 보스몬스터를 구현하기 전에 필요한 Prefab 및 Animation을 위한 fbx 자료 수집
2. Player의 Jump 구현 완료
   - MixAmo에서 적합한 Animation을 찾아서 적용하였음.
   - 사용하려는 Animation은 고정으로 움직이는 것이 아닌 앞으로 점프하는 Animationdmf 이용함
   - 1.움직이는 애니메이션을 고정하기 위해 fbx의 animation에서 Root transform position의 bake into pose를 체크하여 활성화한 후에,
     CharacterController.move()에 쓰이는 Vector3 mv의 y값을 더하여 점프형식을 구현해보려 하였으나 활공 상태가 많이 짧고 앞으로 간후에 제자리로 돌아오는 모션이 
     매우 부자연스러웠음.
     2. 애니메이션은 배제하고 자연스러운 점프 형식을 구현하기 위해 charactercontroller의 isgrounded기능을 이용하여 땅에 닿아있는 경우 Y값을 주고 점프를 구현하려
     고 하였으나 애니메이션이 땅에 떨어져있는 시간이 매우 짧아 애니메이션의 마지막이 실행되기전에 Idle & Run으로 변경되는 일이 발생하였음.
     3. bool 변수를 이용하여 _isJump를 구현함. Jump 애니메이션이 실행 시 _isJump를 True로 바꿔주고 Jump의 애니메이션이 끝나는지점에 Animevent를 이용하여 
     False로 바꾸도록 하였음. false->true로 바뀌는 과정에서 안에 있는 Animation Run , Idle을 관리하는 곳이 발생하기 떄문에 SPace를 누르면 이과정을 넘어가기 위 
     해 return을 사용함.그리고 Jump의 Bake Into Pose를 해제하여 움직이도록 함. 애니메이션의 움직임과 앞으로가는 점프가 자연스러워졌으나 점프 높이가 많이 아쉬움 
     이 남음. 그래서 _isJump가 True일 경우에 CharacterController의 move에 이용되는 Vector3 Mv(X, 0, Z)에 Jump애니메이션이 뛰려고 하는 곳에 AnimEvent를 넣어 
     y에 0.5f의 값을 넣어주었음. 그리고 떨어지는 모션을 취하는 첫 부분에 다시 y를 0으로 바꿔주웠음.
   3번을 이용하여 Player의 점프를 구현하였음.

3.Player가 점프하는 과정에서 카메라의 포커스가 흔들리는 것을 발견하여 이 부분은 CharacterController의 isgrounded를 다시 활용하여 땅에 닿아있는 경우에만 카메라가
  추적할 Player의 Y의값을 받아 카메라가 흔들리는 것을 방지하였음.

2023-0721, 2023-07-24
1. KeyBuffer를 Queue를 이용하여 KeyCode를 받아 원하는 시점에 Attak Keybutton 입력 시 연계 공격을 하도록 구현하려고 하였음.
   - 구현은 하였으나, 원래 사용하던 Keybuffer는 각 연계 Attack에서 Animation Event Clip간의 시간들을 체크해 그 사이에 해당하는 값을 적용받아 연계 공격을 하는거였음.
     그러나 지금 구현하는 방법은 Clip을 가져오기에 부적합한 Animatior와 Event Clip 그리고 함수로 적용시켜놓아서 수정을 하여 적용할지 다시 Reset후 재적용할지 고민이       필요함.
2. 원래 Camera를 PlayerController Script에서 건드렸지만 따로 CameraMovement를 생성하여 Player를 추적하도록 구현함.
   - Camera Object 생성 후, CameraMovement.cs를 붙여 줌. 그리고 MainCamera를 CameraObject에 넣어줌.
     Camera Object는 PlayerObject에 자식 Object인 FollowCam을 추적하도록 하였음. MainCamera는 원하는 만큼 뒤로 후진하여 배치함.
     그 후 카메라가 회전하여 따라다니도록 CameraMovement에서 구현함. (나중에 따로 어떻게 구현하였는지 방식 적기)
     PlayerController에서는 palyer의 컨트롤 부분만 건드리기 위해 Camera 추적 함수들은 지우고  Camera에서 관리하도록 하였음.
     Player가 움직일때 필요한 Camera의 transform은 Tag로 찾아서 구현하기로 하였음.(= Camera Object도 Player의 자식 오브젝트를 찾아야 하므로 동일)

2023-07-25 ~2023-07-26
1. 연계 공격이 더 쉽게 할 수 있도록Animator에 bool IsComobo를 추가하여 마우스 클릭시 공격 연계 하도록 설정하였음 (마우스 클릭을 떼면 IsCombo = false)
   - 여기서 변수가 발생함. MakeTransition을 이용하여 animation 변화를 주어 구현하려고 하였음
     현재 지금 애니메이션 상태를 뚜렷하게 체크하기가 어려움 => Scirpt내에 _CurrentAnyType을 이용하여 현재의 애니메이션이 무엇인지 알아내려고 하였음.
     Has Exit Time을 해제하지 않는 이상 공격 타이밍 시에 _CurrentAnyType을 이용하기 힘들어짐
     또한 연계 Attack1~3을 구현하는데 매우 불편한 상황이 계속 생겨남.
     Animator를 예전에 사용하여 구현했던 방식인 Any State에 연결하여 애니메이션 움직이기 기능을 사용하려고함.
     Make Transtition으로 연결해 놨던 Animation을 정리한 후 Any State에 연결하였고, 해당 이름에 맞춘 Trigger를 생성함.
     Idle - WeaponEquipIdle(Run - WeaponEquipRun 도 포함)은 blend Tree를 이용하여 무기 장착시 IdleKind => 1로 바꿔 무기장착 시 모션으로 바꾸어줌.
     해당 과정을 구현하기 위해 AnimationController -> PlayerController -> PlayerStat -> PlayerController로 상속해줌.
     Player의 Animation는 PlayerAnimation.cs에서 관리하도록 하였고, PlayerStat에서는 Player의 Stat 그리고 PlayerController는 기본적인 컨트롤 기능들을 구현하려고      정리하였음.
     기존에 구현해놓았던 Move 및 Jump 그리고 Attack1 모션 진행까지 수정하여 구현완료함.
     다음부터 Attack1~3까지의 연계공격을 구현만 하면 Player의 기본 움직임 구현완료.

2023-07-27 ~ 2023-08-01
가족 여행 및 친구 여행

2023-08-02
1. Player Animation에서 Attack1~3(기본연계공격)을 구현
   - bool _isPressAtack로 MouseButton Up,Down 별로 True, False를 주어 꾹누르게 될 경우 기본 연계 공격이 나가도록 구현함.
     거기서 KeyBuffer(Queue<KeyCode>)에 타이밍에 맞게 Enqueue로 가져오게 되면 꾹 누르지 않아도 연타클릭으로도 연계 공격이 나가도록 구현함.
   - 무기 장착후에 점프를 하게 되면 공중에 뜨는 현상이 발생하게 되어서, 확인 결과 무기 장착 IDLE에서 BakeIntoPose를 체크를 해주지 않아서 공중에 뜨게됨
     그래서 체크 후 정상적으로 구현함.
2. BossMonster (Gnoll) 구현
   - Gnoll의 Fbx에 Texture, Material를 적용시켜줌. 그런 후에 Fbx를 Unpack Prefab후 Prefab으로 다시 만들어 줌.
   - Animatior에 들어가야할 Animation을 넣어줌.
   - 보스몬스터에 걸맞는 무기를 껴야할 것 같아서 원하는 Mesh와 Material을 적용하여 만든 도끼 Object를 붙여줌.
4. Map1(SetActive)Scene에 MonsterManager 적용
 -각 Map별로 MonsterManager에 적용된 몬스터를 다르게 하여 맵별로 다른 몬스터 생성하도록 할 예정으로 구현해 둠.

2023-08-03
1. Map & Struct 구조 구현 후 Navigation Bake
   - Map에 꾸며줄만한 건물 Object를 생성하여 구현하였습니다. 그리고 Navigation을 이용하기 위해 Bake를 하였습니다.
2. 일반 필드 몬스터를 구현하기 위해 Mushroom몬스터 Object 구현
   - Animatior에 Animation을 연결하였습니다. 
     AnimationController - MonsterAnimController - MonsterStat - MonsterController순으로 스크립트를 상속하도록 하였습니다.
     AnimationController에 있는 AnimationResetting()함수를 Virtual로 만들어 Player와 Monster AnimationController에 Override하여 재정의하여 사용하였습니다.
     MushRoom Object에 NavMeshAgent를 추가하여 MonsterAnimController에 _navAgent를 이용하도록 하였습니다. AnimationResetting() override에 NavMeshAgent를
     GetComponent하였습니다. 애니메이션 모션 중 WALK | RUN 별로 NavMeshAgent의 Speed를 다르게 주도록 SerializedField를 구현해 놓았습니다. 상속받은 
     MonsterController의 Inpsector창에 보이는지 확인하였습니다. 그리고 MonsterController에 _limit (float)변수를 만들어 본인의 위치에서 사각의 형태를 만들어
     사각형 안의 랜덤 포지션으로 움직여 돌아다니도록 구현하였습니다.
