using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Dictionary
{
    class Time
    {
        long curTime = 0;
        static public float deltaTime { get; private set; }

        public void Start()
        {
            curTime = DateTime.Now.Ticks;
        }

        public void Update()
        {
            long delta = DateTime.Now.Ticks - curTime;
            curTime = DateTime.Now.Ticks;
            deltaTime = delta / 10000000f;
        }
    }

    class GameFrameWork
    {
        
        enum eSlotItem
        {
            None,
            ChainAttack,
            AllClear,
            TimeStop,
            SlowTime,
            FeverTime,
            RandomAllChange,
            DoubleAttack,
            HighRiskHighReturn
        }

        public bool isPlay { get; private set; }
        float playTime = 0.0f;
        float itemTime = 0.0f;

        float firstLevelAppearTime = 2.0f;
        float appearTime = 0.0f;
        float velocity = 1;
        bool reDraw = false;
        int score = 0;
        int point = 10;
        int feverPoint = 2;
        bool feverFlag = false;
        int level = 1;
        int levelCheck = 0;
        int itemCheck = 0;
        int doubleAttackCnt = 10;

        eSlotItem[] itemSlots = new eSlotItem[2];
        eSlotItem usedItem = eSlotItem.None;
        eSlotItem usingItem = eSlotItem.None;

        List<int> list = new List<int>();
        Random rnd = new Random();

        public GameFrameWork()
        {
            isPlay = true;
            appearTime = firstLevelAppearTime;
            velocity = 1 / appearTime;
        }

        public void InputProcess()
        {
            if (Console.KeyAvailable) 
            { 
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int inputNum = (int)keyInfo.Key - (int)ConsoleKey.NumPad0;

                /*switch (keyInfo.Key)
                {
                    case ConsoleKey.NumPad1:
                        DeleteNumber(1);
                        break;
                    case ConsoleKey.NumPad2:
                        DeleteNumber(2);
                        break;
                    case ConsoleKey.NumPad3:
                        DeleteNumber(3);
                        break;
                    case ConsoleKey.NumPad4:
                        DeleteNumber(4);
                        break;
                    case ConsoleKey.NumPad5:
                        DeleteNumber(5);
                        break;
                    case ConsoleKey.NumPad6:
                        DeleteNumber(6);
                        break;
                }*/

                switch (usedItem)
                {
                    // Chain = 아이템 사용 후 입력한 숫자에 해당되는 모든 숫자 삭제
                    case eSlotItem.ChainAttack:
                        list.RemoveAll(n => n == inputNum);
                        usedItem = eSlotItem.None;
                        reDraw = true;
                        break;
                    // All Clear = 추가되어 있는 모든 숫자를 지우고 그만큼 점수를 증가 시킨다
                    case eSlotItem.AllClear:
                        DeleteNumber(inputNum);
                        int tmpCnt = list.Count();
                        score += (tmpCnt * 10);
                        list.Clear();
                        usedItem = eSlotItem.None;
                        reDraw = true;
                        break;
                    // Time Stop = 5초간 시간 정지
                    case eSlotItem.TimeStop:
                        DeleteNumber(inputNum);
                        StopTimer(5.0f);
                        usedItem = eSlotItem.None;
                        reDraw = true;
                        break;
                    // Slow Time = 10초간 레벨 1의 시간으로 숫자 추가
                    case eSlotItem.SlowTime:
                        DeleteNumber(inputNum);
                        itemTime = 10.0f;
                        usingItem = eSlotItem.SlowTime;
                        usedItem = eSlotItem.None;
                        reDraw = true;
                        break;
                    // Fever Time = 5초간 획득 점수 두배
                    case eSlotItem.FeverTime:
                        DeleteNumber(inputNum);
                        itemTime = 5.0f;
                        feverFlag = true;
                        point *= feverPoint;
                        usingItem = eSlotItem.FeverTime;
                        usedItem = eSlotItem.None;
                        break;
                    // Random All Change = 모든 숫자를 랜덤한 하나의 숫자로 변경
                    case eSlotItem.RandomAllChange:
                        DeleteNumber(inputNum);
                        int tmp = rnd.Next(1, 7);
                        int cnt = list.Count;
                        list.Clear();
                        for (int i = 0; i < cnt; i++) list.Add(tmp);
                        usedItem = eSlotItem.None;
                        reDraw = true;
                        break;
                    // Double Attack = 10회간 2개씩 삭제
                    case eSlotItem.DoubleAttack:
                        DeleteNumber(inputNum);
                        doubleAttackCnt = 10;
                        usingItem = eSlotItem.DoubleAttack;
                        usedItem = eSlotItem.None;
                        reDraw = true;
                        break;
                    // High risk high return = 5초간 속도가 현재 속도에 비례해 2배가 빨라진다. 획득하는 점수는 10배
                    case eSlotItem.HighRiskHighReturn:

                        break;
                    default:
                        if (keyInfo.Key == ConsoleKey.LeftArrow)
                        {
                            UseItem(itemSlots[0]);
                            itemSlots[0] = eSlotItem.None;
                        }
                        else if (keyInfo.Key == ConsoleKey.RightArrow)
                        {
                            UseItem(itemSlots[1]);
                            itemSlots[1] = eSlotItem.None;
                        }
                        else DeleteNumber(inputNum);
                        break;
                }
                
            }
        }

        public void Update()
        {
            playTime += Time.deltaTime;

            if (usingItem == eSlotItem.SlowTime || usingItem == eSlotItem.FeverTime) UsingItem();
            else if (playTime >= appearTime)
            {
                playTime -= appearTime;
                AddNumber();
            }

        }

        public void UsingItem()
        {
            itemTime -= Time.deltaTime;

            if(itemTime <= 0.0f)
            {
                usingItem = eSlotItem.None;
                if (feverFlag == true)
                {
                    point /= feverPoint;
                    feverFlag = false;
                }
            }
            else if(usingItem == eSlotItem.SlowTime)
            {
                if(playTime >= firstLevelAppearTime)
                {
                    playTime -= firstLevelAppearTime;
                    AddNumber();
                }
            }
            else if(usingItem == eSlotItem.FeverTime)
            {
                if(playTime >= appearTime)
                {
                    playTime -= appearTime;
                    AddNumber();
                }
            }

        }

        public void Draw()
        {
            if (reDraw != true) return;
            Console.Clear();
            //foreach (var i in list) Console.Write($"[{i}]");
            Console.WriteLine($"점수 : {score} 단계 : {level} 속도 : {velocity}");

            switch (usedItem)
            {
                case eSlotItem.ChainAttack:
                    Console.WriteLine("<체인어택> : 지우고 싶은 숫자를 선택하세요");
                    break;
                case eSlotItem.AllClear:
                    Console.WriteLine("<올클리어> : 화면에 보이는 숫자를 전부 삭제합니다");
                    break;
                case eSlotItem.TimeStop:
                    Console.WriteLine("<타입스탑> : 시간을 5초간 정지합니다");
                    break;
                case eSlotItem.SlowTime:
                    Console.WriteLine("<슬로타임> : 10초간 레벨 1의 속도로 변경됩니다");
                    break;
                case eSlotItem.FeverTime:
                    Console.WriteLine("<피버타임> : 5초간 획득 점수가 두배가 됩니다");
                    break;
                case eSlotItem.RandomAllChange:
                    Console.WriteLine("<랜덤교환> : 모든 숫자를 랜덤한 하나의 숫자로 변경");
                    break;
                case eSlotItem.DoubleAttack:
                    Console.WriteLine("<더블어택> : 다음 10번의 입력 동안 2개씩 삭제합니다");
                    break;
                case eSlotItem.HighRiskHighReturn:
                    Console.WriteLine("<탐려득주> : 5초간 속도가 현재 속도에 비례해 2배가 빨라진다. 획득하는 점수는 10배");
                    break;
                default:
                    Console.WriteLine();
                    break;
            }
            //Console.WriteLine($"속도 : {noteSpeed}");
            for (var i = list.Count - 1; i >= 0; i--)
            {
                Console.Write($"[{list[i]}]");
            }
            Console.WriteLine();
            Console.WriteLine();
            foreach(var item in itemSlots)
            {
                switch (item)
                {
                    case eSlotItem.None:
                        Console.Write("[비어있음]");
                        break;
                    case eSlotItem.ChainAttack:
                        Console.Write("[체인어택]");
                        break;
                    case eSlotItem.AllClear:
                        Console.Write("[올클리어]");
                        break;
                    case eSlotItem.TimeStop:
                        Console.Write("[타입스탑]");
                        break;
                    case eSlotItem.SlowTime:
                        Console.Write("[슬로타임]");
                        break;
                    case eSlotItem.FeverTime:
                        Console.Write("[피버타임]");
                        break;
                    case eSlotItem.RandomAllChange:
                        Console.Write("[랜덤교환]");
                        break;
                    case eSlotItem.DoubleAttack:
                        Console.Write("[더블어택]");
                        break;
                    case eSlotItem.HighRiskHighReturn:
                        Console.Write("[탐려득주]");
                        break;
                }
            }
            reDraw = false;
        }

        void AddNumber()
        {
            if (list.Count == 10)
            {
                isGameOver();
                return;
            }
            int a = rnd.Next(1, 7);
            list.Add(a);
            reDraw = true;
        }

        void DeleteNumber(int a)
        {
            if (list.Count == 0) return;
            if (list[0] == a)
            {
                if (usingItem == eSlotItem.DoubleAttack)
                {
                    list.RemoveAt(0);
                    score += point;
                    levelCheck += point;
                    if (list.Count >= 1) { list.RemoveAt(0); score += point; levelCheck += point; }
                    doubleAttackCnt--;
                    reDraw = true;
                    if (doubleAttackCnt <= 0) usingItem = eSlotItem.None;
                }
                else 
                {
                    score += point;
                    levelCheck += point;
                    list.RemoveAt(0);
                    reDraw = true;
                }
                LevelUp();
                ItemCheck();
            }
            else
            {
                score = (score - point) < 0 ? 0 : (score - point);
                reDraw = true;
            }
        }

        void isGameOver()
        {
            Console.Clear();
            Console.WriteLine("게임 오버!");
            isPlay = false;
        }

        void LevelUp()
        {
            if(levelCheck >= 150)
            {
                levelCheck -= 150;
                level++;
                appearTime *= 0.9f;
                velocity = 1 / appearTime;
            }
        }

        void ItemCheck()
        {
            if(++itemCheck >= 4)
            {
                itemCheck = 0;
                AddItem();
            }
        }

        void AddItem()
        {
            for(int i = 0; i<itemSlots.Length; i++)
            {
                if (itemSlots[i] == eSlotItem.None)
                {
                    //itemSlots[i] = eSlotItem.DoubleAttack;

                    eSlotItem rndSlotItem = (eSlotItem)rnd.Next(1, 7);
                    itemSlots[i] = rndSlotItem;
                    reDraw = true;
                    break;
                }
            }
        }

        void UseItem(eSlotItem item)
        {
            reDraw = true;
            switch (item)
            {
                case eSlotItem.ChainAttack:
                    usedItem = eSlotItem.ChainAttack;
                    break;
                case eSlotItem.AllClear:
                    usedItem = eSlotItem.AllClear;
                    break;
                case eSlotItem.TimeStop:
                    usedItem = eSlotItem.TimeStop;
                    break;
                case eSlotItem.SlowTime:
                    usedItem = eSlotItem.SlowTime;
                    break;
                case eSlotItem.FeverTime:
                    usedItem = eSlotItem.FeverTime;
                    break;
                case eSlotItem.RandomAllChange:
                    usedItem = eSlotItem.RandomAllChange;
                    break;
                case eSlotItem.DoubleAttack:
                    usedItem = eSlotItem.DoubleAttack;
                    break;
                case eSlotItem.HighRiskHighReturn:
                    usedItem = eSlotItem.HighRiskHighReturn;
                    break;
                default:
                    break;
            }

        }

        void StopTimer(float t)
        {
            playTime += Time.deltaTime;
            while(playTime >= t)
            {
                playTime += Time.deltaTime;
            }
            playTime -= t;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 프레임 워크
            GameFrameWork gfw = new GameFrameWork();
            Time time = new Time();
            time.Start();

            while (gfw.isPlay)
            {

                //long elapsedTicks = DateTime.Now.Ticks - curDate.Ticks;
                //TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

                //float deltaTime = elapsedTicks / 10000000f;
                // 초당 이동 거리
                // 100 프레임기준 프레임당 이동거리 1 * 0.01
                // 20 프레임기준 프레임당 이동거리 1 * 0.05

                //for (int i = 0; i < 10000; i++) { int n = i * i; }

                //Console.WriteLine(deltaTime);
                Thread.Sleep(100);
                time.Update();

                gfw.InputProcess();
                gfw.Update();
                gfw.Draw();

                // 숫자 디펜스 게임
                // 일정한 시간마다 1~6까지의 랜덤한 숫자가 추가된다
                // 현재 남은 숫자가 10개 이상이 되면 게임 오버
                // 현재 숫자중 가장 먼저 추가된 숫자를 입력하면 해당 숫자는 삭제된다
                // 숫자를 지울 때 마다 10점을 얻는다
                // 150점을 얻을 때 마다 레벨이 증가한다
                // 레벨이 증가하면 숫자의 추가 속도가 빨라진다
                // 잘못된 입력을 하면 점수를 10점 깎는다

                // Item
                // 8개의 숫자를 지울때 마다 아이템을 얻는다
                // Item은 2개까지 저장 가능
                // Item slot 은 2개, 왼쪽 화살표키는 왼쪽 슬롯, 오른쪽 화살표키는 오른쪽 슬롯
                // All Clear = 추가되어 있는 모든 숫자를 지우고 그만큼 점수를 증가 시킨다
                // Time Stop = 5초간 시간 정지
                // Slow Time = 10초간 레벨 1의 시간으로 숫자 추가
                // Fever Time = 5초간 획득 점수 두배
                // Random All Change = 모든 숫자를 랜덤한 하나의 숫자로 변경
                // Double Attack = 10회간 2개씩 삭제
                // Chain = 아이템 사용 후 입력한 숫자에 해당되는 모든 숫자 삭제
                // High risk high return = 5초간 속도가 현재 속도에 비례해 2배가 빨라진다. 획득하는 점수는 10배

                // Changed
            }

        }
    }
}
