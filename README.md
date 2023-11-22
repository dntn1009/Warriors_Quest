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
     Player의 Animation는 PlayerAnimation.cs에서 관리하도록 하였고, PlayerStat에서는 Player의 Stat 그리고 PlayerController는 기본적인 컨트롤 기능들을 구현하려고        정리하였음.
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

2023-08-07, 2023-08-10 ~ 2023-08-11
게임을 만드는 과정에서 편하게 구현하기 위해서 구현과정 순서를 정리하는데에 중점을 두고 정리하였습니다. 그 후에 다시 구현에 들어갔습니다.
1.Player & Monster Stat 임시 구현
  -Player와 Monster의 공격시 Demage 및 크리티컬, 회피, 방어율과 데미지의 연관관계를 이용해서 구현하기 위해 스텟을 임시 
  로 구현 하였습니다. Stat Script를 생성 후에 필요한 변수와 프로퍼티를 구현하여 각 Player & Monster Stat Script에 넣어 
  주었습니다.
2..Player & Monster Attack 구현
  - Noraml Attack - Critical Attack Decision을 이용하여 일반 데미지를 줄지 크리티컬 데미지를 줄지 몬스터가 회피할지를 선택
    하여 데미지를 주도록 하는 Process함수를 만들어 데미지를 주도록 구현하였습니다. AttackType을 이용하여 UI표시로 일반과 크리
    티컬 공격시에 나타나는 이펙트도 다르게 구현하기 위해 Enum으로 생성하였습니다. 그리고 Monster가 피격 시 Monster안에 있는
    오브젝트인 Monster_Hit부분에 나타나도록 하기위해 Util 스크립트에 공용으로 쓸 수 있는 타겟오브젝트안에있는 자식오브젝트 찾
    기 함수를 구현하였습니다. 공격 함수들 또한 Util에 구현해 놓아 편하게 쓰도록 하였습니다.
3.Monster BehaviourState 기초 구현
  - 몬스터가 알아서 움직이도록 구현하기 위해 FSM을 이용하기 위하여 Behaviour enum을 만들어 Behaviour Process 함수를 구현하였
    습니다. 기초적인 부분만 구현해 놓았습니다.
4.Monster 생성시 필요한 ObjectPool 스크립트 생성 및 구현
  - GameObjectPool 스크립트를 생성하여 몬스터를 생성하는 과정에서 낭비를 막기 위하여 구현하였습니다. MonsterManager에서 쓰일 
    예정입니다.
5.MonsterManager 스크립트 생성
  - 몬스터의 전반적인 관리를 하기위한 스크립트를 생성하였습니다.

2023~08-23
1.Monster FSM 구현 완료.
 - Player와 Monster의 Attack & Demage를 주기 위하여 함수를 구현 적용 완료하였습니다.
 - Monster FSM 과정 중에 Attack해야하는 경우를 위하여 Player & MAP의 LAYER를 이용하여 RAYCAST를 쏘아 계산한 거리에 적중한 것이 Player일 경우 ATTACK &FSM 애니메이션이 적용되도록 구현함.
 - PlayerHit, MonsterHit이라는 자식 오브젝트를 각각 넣어주어 그 Position에 Fx효과를 주려고 하였음. 몬스터를 크리티컬 & 일반 공격시 이펙트가 나가도록 구현함.

2. Player & Monster Attack 관련 Script (AttackAreUnitFind, Controller의 Collider관련 부분) 방식 변경
   - 몬스터나 Player가 공격할 경우에 해당 Target을 맞출 수 있음에도 불구하고 Demage가 안들어가는 경우가 발생함.
   - 찰나의 순간이라 Collider로 인식하는 경우가 안좋다는걸 느끼게 되었음.

2023-08-24
1. Monster의 Attack부분에 안좋은 점이 있는 것을 발견하여 다시 구현
   - BoxColider에서 타겟을 잘못 잡아서 demage를 입히지 못하는 상황이 발생하여. UnitList를 추가하여 BoxCollider를 켜놓은 상태에서 사냥하도록 구현하였습니다.

2023-08-25 ~ 2023-08-30
1. Monster를 Map에 생성하기 위한 Script & gameobject(transform position) 작업 및 구현
   - MonsterController에서 InitMonster()함수를 이용하여 몬스터 본인의 첫 포지션과 범위내 활동을 위한 포지션을 초기에 세팅하도록 함.
     맵별로 ActiveScene을 이용하여 만들 예정이므로 싱글턴을 부모로 삼는 MonsterManager스크립트를 생성하여 몬스터 프리팹들과 몬스터를 미리 생성하도록 함
     그래서 게임오브젝트풀링을 이용하였고 각 몬스터 프리팹마다 지정한 숫자만큼 생성하도록 구현함.
     그리고 맵별로 GenPosition이라는 Empty Object를 생성하여 몬스터를 소환하고 싶은 위치를 정해 놓았습니다. 그리고 MonsterManager에 Transform 배열을 받아와
     _genCheck라는 Bool 배열을 만들어 해당 위치가 false일 경우 그위치에 소환하도록 하였습니다.

~2023-09-04
1. IngameManager & MonsterManager를 연계하여 몬스터 관련 부분 구현
   - IngameManager는 SetActiveScene 밖에 있는 스크립트로 인게임 내 총괄 관리 스크립트로 사용하기 위하여 만들었습니다.
     현재 맵을 확인하기 위한 함수와 변수가 있고 맵에 최대 소환할 수 있는 몬스터 숫자와 계속 몬스터를 생산하기 위한 함수와 코루틴 함수가 있습니다.
     그리고 현재 몬스터가 생성중이면 잠깐 코루틴을 중지시키기 위하여 따로 코루틴 변수(?)를 만들어 놓고 Null이 되면 다시 생성하도록 하였습니다.
     맵이 변경 될때마다(SecActiveScene) 최대 몬스터 숫자와 현재 맵이 변경되도록 구현하였습니다.
     정리하자면 MapState() 함수로는 실행중인 코루틴이 있는 경우에는 바로 return 하도록 하였으며
     만약 현재 스폰한 몬스터가 최대 스폰 몬스터 수보다 적을 경우 스폰한 몬스터 += 1을 해주고 코루틴을 작동하도록 하였습니다.
     코루틴으로는 현재 MonsterManager에 있는 bool _genCheck[]를 이용하여 false인 부분을 찾고 거기에 true를 해주고 그 해당 배열구역을저장하여 그 _genPosition[찾은
     부분]의 transform 위치에 소환시켜주도록하였습니다.
 ~2023-09-07
1. Monster HUD Controller & 몬스터 HPBAR 구현
   - UGU를 이용하여 HUD를 만들거나 다른 UI를 구현할 경우에 Canvas를 하나만 이용해서 만들려는 생각은 아주 안좋고 부적절한 습관이자 생각이라고 함.
     그래서 WorldPositon으로 구현한 Monster HUD Canavs에 몬스터의 이름과 HP가 나오도록 구현하였습니다.
     그리고 몬스터가 피격시 HPBar가 닳는 현상을 구현하였습니다.
     그리고 몬스터와 전투상태가 풀리거나 공격한지 3초가 지난 후면 HPBar가 사라지도록 구현하였습니다.
     애초에 피격하지 않은 상태면 HPBar가 뜨지 않은 상태로 되어있습니다.
2. Monster HUD DemageUI 구현
   - 데미지를 입을 시에 Monster Object에서 Hit object의 위치에 데미지 숫자가 나오도록 구현하였습니다.
     각 크리티컬 노말 별로 데미지가 나오도록 구현하였으며 플레이어의 데미지는 색깔이 다르게 나오도록 구현하였습니다.
     데미지가 위로올라가면서 사라지는 등의 애니메이션을 커브를 이용하여 구현하였습니다.
     따로 demage Canvas가 생성되는 부분을 만들어 지저분하지 않게 관리하도록 하였습니다.

 ~2023-09-14
 1. Inventory 구현
    - 스크립터블 오브젝트를 이용하여 아이템의 성질을 정한 Object를 구현하였습니다.
      그리드 레이아웃을 이용하여 아이템칸을 정해두었습니다. Item : 30 HotKeyItem : 7 EquipItem : 5(Head, Chest, Legs, Feet, Weapon)
      그리고 Raw Image와 Render Texture 그리고 Player Object의 자식에 Camera를 설치하여 Inventory에 아이템을 착용 시 변하는 모습을 보여주기 위하여
      현재 Player의 모습을 띄워놓았습니다.
      그리고 Cursor를 잠궈놓은 상태지만 Inventory를 열 경우에는 커서가 생기고 화면이 회전하거나 플레이어가 공격하는 모션등이 구현되지 return하였습니다.
      (포인터 및 아이템 터치 이동 기능 등은 다시 정리하여 적기로 함)

 ~2023~10~10
 1. Inventory & item & spawnItem & EquipItem(skinnedBones) 구현
    - Inventory의 EquipSlot 칸을 추가하였습니다. (Head, Chest, Legs, Feet, Gloves, Shoulders, Weapon)
    - Item의 Info를 알려주기 위한 부분을 Inventory의 오른쪽 상단에 구현하였습니다.
    - 원하는 EquipItem을 장착하게 되면 해당 장비의 외형과 능력치가 적용되도록 구현하였습니다.
      포인터로 해당 Equip SLot 부분에 장비를 착용시 Player Obj에 추가한 PlayerEquipmentInfo 스크립트에서 외형 부분은 (Item에 있는 Str, Player의 Amor 외형 Obj)
      으로 딕셔너리를 만들어 Setactvie가 true가 되도록하였습니다. 그리고 item에 있는 att or def 부분의 정보를 가져와 Player의 stat에 적용시켜주었습니다.
    - Player의 자식 obj 중 Amor obj가 skinned mesh에 있는 bones가 연결되어있지 않아 안뜨는 현상 수정하여 구현 완료.
      skinned mesh의 inspector창에는 보이지 않는 bones를 skinnedmeshrendererinfo scirpt를 만들어 그 안에 bones를 연결하여 붙여지도록 구현하였습니다.
    - potion의 구매량에 따라 원래 있던 포션의 개수를 Max로 채우고 나머지 빈칸에 남은 구매량의 potion이 생성되도록 구현하였습니다(노말 템 또한 가능)
      장비 및 단일개수 아이템들은 1개로 고정하여 개수가 뜨지 않도록 구현하였습니다.
  2. Player & Monster & Map의 크기를 변경하였습니다.
     - 원래는 scale을 0.1로 맞추고 terrain의 크기를 128로 고정하려 하였으나 여러부분에서 크기가작아 불편해지는 상황이 많아져서 terrain맵 기준을 1000으로 만들어
       scale의 크기를 다시 재조정 하였습니다. 그에 맞춰 obj의 비율을 재설정 하였습니다.
  3. Player Hud 및 현재 타격하고있는 Monster Status Hud UI 구현
     - Player Hud를 표시하기위한 Ui를 왼쪽 상단에 만들어 두었으나 skill의 쿨타임확인과 인벤토리, 스텟, 스킬창 등등의 Page를 확인하기 위한
       Ui창을 열게될떄 필요한 Button들이 있어야 하기 때문에 하단에 일자로 만들 예정입니다.
     - Monter obj 마다 각 체력바 및 이름이 표시되어 있지만 현재 타격하는 몬스터의 정보를 크게 확인하기 위하 상단 가운데에 타격중인 몬스터의 정보를 확인하는
       Ui를 구현하였습니다.
  4. Inventory창에 들어가거나 Cursor가 보일 경우 조종 중인 Player의 Animation이 IDLE로 변경 및 Player 조종 불가 상태로 구현하였습니다.
  5. EQuip slot에서 Weapon 착용 시에 Player의 Animation 변경 및 다른 세세한 부분을 구현하였습니다.

~2023-10-21
1. Monster 관련 부분 수정 및 구현
 - Monster Stat부분에 관련하여 Player와 동일하게 쓰고있었지만 MStat.Script으로 몬스터 스텟 추가 구현.
   Monster HUD에 HP가 0이여도 피가 조금 남아보이는 현상 수정
   Monster의 AI가 이상하여 공격을 하지 않는 행위, Idle상태로 죽기, 움직이지 않는 행위 등이 있어 Controller에서 BehaviourState를 다시 코딩하여 어색하지 않게 구현
   Monster가 플레이어와 부딪히면 플레이어가 밀리는 현상이 발생하여 Monster의 NavAgent가 Player의 Nav보다 단계를 낮추어 피해다니도록 구현하였습니다. 공격 시에는 피하지 않습니다.
   Monster의 UI부분이나 스텟 적용이 잘 안되는 상황이 있었지만 능력치는 Awake에서 받도록 했지만 함수로 몬스터를 생성하는 경우에 설정하여 에러가 뜨는 것으로 수정완료하였습니다.
2. Player가 공격후에 제자리로 돌아오는 어색한 부분을 수정하였습니다.
   - 공격 하는 부분에 부자연 스러운 부분을 개선하였습니다. 싸우는 과정에서 보이는 Hit Effect를 수정하였습니다.
3. 다 완성된 Map 주변에 Player가 떨어지면 안되는 구역들을 연결하여 콜라이더 벽을 만들었습니다.
  - WallColliderGenerator라는 script 오픈소스를 이용하여 떨어지면 안되는 부분들의 콜라이더 벽을 만들었습니다.
    출처 : https://www.youtube.com/watch?v=5yMvoA2Gp-Q
4. NPC와 구조물들 생성 및 구현
  - NPC와 구조물들의 Asset을 Unity Asset Store에서 찾아와 맵을 꾸며주었습니다.
5. Top & Bottom UI 구현
  - 보기 안좋은 UI를 개선하기 위해 Top & Bottom 별로 구현하였습니다. Top에는 몬스터와 전투 시에 보이는 정보들과 인벤토리, 스텟, 스킬, 메뉴, 퀘스트 창 등 그리고 MAP을 표시하도록 정해두었습니다.
    Bottom에는 Player의 체력 및 MP, Exp와 인벤토리의 Hot Key Slot에 등록한걸 보여줄 수 있는 칸들과 스킬 Slot 칸들을 구현해 놓았습니다.
    
~2023-10-31
1. MiniMap과 Map을 구현
 - 구현하기 위해 RenderTexture를 이용하였습니다. 그래서 위에서 맵 전체를 찍고 있는 Camera와 Player를 따라다니는 Camera를 생성하였습니다.
 - 그리고 MiniMap과 Map의 카메라에는 Ground만 보이도록 설정하였습니다.
2. 퀘스트 NPC, 상정 NPC, 대화 NPC, Player를 Map에 원으로 간편하게 보일 수 있도록 구현
   - Player와 NPC에 MapMaking이라는 동그란 원형의 Object를 만들어 자식오브젝트로 넣었습니다. 각각 Minimap과 Map의 Makgin Object입니다.\
   - 각 Tag&Layer의 이름이나 역할에 따라 색깔을 구분지었습니다. 그리고 MainCamera에는 Layer인 MiniMap, Map부분을 보지 않도록 설정하였으며,
   -  맵과 미니맵의 RenderTexture를 보여주고 있는 Camera들에는 Minimap과 Map의 마킹을 보이도록 구현하였습니다.
3. Skill 구현하기
 - BuffSKill 1개와 공격스킬 2개를 구현하도록 하였습니다.
 - 장판Effect, Demage, cooltime을 가지고있는 스크립터블 오브젝트 skilldata를 playercontroller안에 변수로 넣어두었습니다.
 - 또한 플레이어의 스킬과정중에 필요한 Animation를 찾아 넣어두었고 해당 스킬 키를 누르면 애니메이션이 작동하도록 구현하였습니다.
 - 코루틴을 이용하여 해당 스킬을 사용하면 그 시간동안 스킬을 사용하지 못하도록 하였습니다.
 - 그리고 Animation중에 몬스터가 피격하면 데미지가 닳도록 AnimEvent_AttackSkill 메서드를 구현하였습니다. JumpAttackSkill은 이과정에서 터지는 장판 효과를 생성합니다.
4. 검기(웨펀트레일) & 공격 Skill 사용시 무기에 Effect 효과 적용 및 구현
   - 플레이어가 공격할때 무기에 검기가 생성되도록 구현하기 위해 Trail Renderer라는 Component를 이용하였습니다.
   - 처음에는 웨펀 트레일 렌더러라는 간편한 기능이 구현되어있는지 몰라서 많이 헤매는 과정에서 시간이 오래 걸렸습니다.
   - 기존에 받아두었던 Particle을 이용한 Effect효과들 중에서 마음에드는 부분을 골라 편집하여 스킬을 사용할때 무기주변에서 빛나거나 붙타도록 구현하였습니다.

~2023-11-13
1.WindowUI창 구현
-WindowUI Canvas를 구현하기 전에 따로 InventoryWindow, StatWindow, SkillWindow 등 각자 Canvas를 만들었으나 매우 불편하여
 WindowUI Canvas안에 넣어 쓰도록 변경하였습니다. 이 과정에 따라 그동안 구현하였던 부분에 불필요한 부분이나 IngameManager(SIngleTon)에 SerializedField를 다시 넣어 두었습니다.
 각 Window.Script에 Player의 정보가 필요하다면 Serialized Field로 적용하여 구현하였습니다.
 또한 Window창을 움직이게 하기위해 WindowMove.Script를 생성하였습니다. Header라는 Object(Button)이 Window의 상단에 있도록 두었고 Close_Button또한 Header의 오른쪽에 있도록 구현하였습니다.
 WindowMoove 스크립트에는 부모Obj의 RectTransform과 클릭Down시 본인의 첫 위치를 아는 메서드가 있습니다. 그리고 OnDragHandler를 이용하여 인터페이스 안에 첫 클릭한 부분에서 움직이는 부분대로 본체가 움직이도록 구현하였습니다.
 EventTrigger를 이용하여 버튼을 누른 경우에 위치를 알도록 구현하였습니다.
2.Fixed Update이용
-물리 엔진을 사용해야 할때 활성화 되는 점과 update는 불규칙한 호출이라는 점을 알게되어 캐릭터의 움직임이나 움직임 관련 부분들은 FixedUpdate 부근으로 변경하였습니다.
3. Mouse Cursor문제 해결
- Mouse를 이용하는 게임이기 때문에 Window창을 볼때를 제외하고는 Mouse 커서가 잠궈져있고 보이지 않는 상태여야 합니다. 그리고 플레이어는 마우스 커서가 보이면 움직이지 않도록 해야합니다.
  이 과정에서 많이 부자연스러웠던 부분들을 다시 찾아 해결하였습니다. 그리고 이과정에서 마우스 커서로 인벤토리, 스텟, 스킬, 메뉴 버튼을 누르면 Window창이 켜지는게 아니라 그대로 안보이도록 구현되어있는것을 발견하였고
  GetMouseButtonDown(0)으로 눌렀을 때 RayCastHit을 쏘는 과정에서 EventSystem.current.IsPointerOverGameObject()메서드를 이용하여 False인 경우에만 잠그도록 구현하였습니다.
  IsPointerOverGameObject()는 UI에 쏘아지면 True인 특성을 가지고 있는 메서드 입니다.
 
 
