# ⚔️ Warriors_Quest

Unity로 제작한 퀘스트 중심의 액션 RPG 게임 프로젝트입니다.  
몬스터 처치, 아이템 수집, 장비 장착 등 RPG 핵심 시스템과  
씬 전환, 미니맵, 퀘스트 진행 UI까지 포함한 **1인 개발 포트폴리오 프로젝트**입니다.

---

## 📆 개발 기간  
> **2023.11 ~ 2024.03 (약 5개월)**  
> 개인 개발 (기획 + 개발 전부 담당)

---

## 🧩 주요 시스템

- 퀘스트 수락 / 진행 / 완료 시스템 (킬 퀘스트, 수집 퀘스트 지원)
- 몬스터 FSM (IDLE / PATROL / CHASE / ATTACK / DEATH)
- 플레이어 스탯, 장비 시스템, 포션 사용 등 RPG 기본 요소 포함
- **인벤토리 / 장비창 / 퀘스트창 / 스탯창** 등 전체 UI 구현
- **미니맵 & 마커**, **맵 전환**, **Scene 이동 처리 시스템**
- **Google Sheet → JSON → ScriptableObject**를 통한 데이터 연동 구조

---

## 🛠 사용 기술 스택

- Unity 2022.3.x
- C# / ScriptableObject / PlayerPrefs
- JSON 기반 데이터 관리
- FSM 패턴 / 싱글턴 구조
- 커스텀 인벤토리 & UI 시스템

---

## 🔁 주요 기능 미리 보기

- ✔ 퀘스트 시스템  
  Kill / Gathering 타입 분기 처리, UI 연동, 완료 보상 지급까지 구성
- ✔ 몬스터 AI  
  상태 기반 FSM으로 구현된 몬스터 AI (Slime / Mush / Gnoll 등)
- ✔ UI 통합 관리  
  UIManager를 통한 창 전환 및 상태 제어 처리
- ✔ 씬 전환 시스템  
  Additive 방식 + SetActiveScene(), 몬스터 자동 재소환 코루틴 포함
- ✔ 미니맵 / 마커  
  마커 색상 구분, 카메라 Follow 방식으로 추적

---

## 📂 프로젝트 구조 예시

```plaintext
Assets/
├── Scripts/
│   ├── Inventory/              # 인벤토리 및 아이템 로직
│   ├── Quest/                  # 퀘스트 관련 데이터 및 목표 체크
│   ├── Monster/                # 몬스터 FSM / 스탯 / 리워드 시스템
│   ├── UI/                     # StatWindow / MapWindow / QuestWindow 등
│   └── Managers/               # IngameManager / DataManager / AudioManager
├── Resources/
│   ├── ScriptableObjects/      # QuestData, ItemData, MonsterData 등
│   └── JSON/                   # 시트 연동용 JSON
├── Scenes/                     # Ingame / Stage1 / Stage2
 
