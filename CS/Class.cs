using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace test
{
    public enum eGender { MALE, FEMALE }

    abstract class Animal
    {

        protected string _name;
        protected int _age;
        protected int _maxAge;
        protected eGender _gender;
        protected float _weight;

        public Animal(int mA)
        {
            _age = 1;
            _maxAge = mA;
            Random rnd = new Random();
            _gender = (eGender)rnd.Next(0, 2);
            Console.WriteLine("동물이 태어났습니다");
        }

        public abstract void GetStat();
        //public abstract void SetStat();

        public string Name { get; set; }

        public abstract void Sleep();
        public abstract void Eat();
        public abstract void Cry();
    }
    class Dog : Animal
    {
        public Dog() : base(10)
        {
            _weight = 1.32f;
            Console.WriteLine("태어난 동물은 개입니다");
        }

        public override void GetStat()
        {
            Console.WriteLine($"Status");
            Console.WriteLine($"1) Age : {_age}");
            Console.WriteLine($"2) Max age : {_maxAge}");
            Console.WriteLine($"3) Gender : {_gender}");
            Console.WriteLine($"4) weight : {_weight}");
        }
        public override void Sleep() { Console.WriteLine($"개가 잠을 잡니다"); }
        public override void Eat() { Console.WriteLine($"개가 음식을 먹습니다"); }
        public override void Cry() { Console.WriteLine($"개가 포효합니다"); }
    }

    class Cat : Animal
    {
        public Cat() : base(8)
        {
            _weight = 0.97f;
            Console.WriteLine("태어난 동물은 고양이입니다");
        }

        public override void GetStat()
        {
            Console.WriteLine($"Status");
            Console.WriteLine($"1) Age : {_age}");
            Console.WriteLine($"2) Max age : {_maxAge}");
            Console.WriteLine($"3) Gender : {_gender}");
            Console.WriteLine($"4) weight : {_weight}");
        }
        public override void Sleep() { Console.WriteLine($"고양이가 잠을 잡니다"); }
        public override void Eat() { Console.WriteLine($"고양이가 음식을 먹습니다"); }
        public override void Cry() { Console.WriteLine($"고양이가 웁니다"); }
    }

    class Program
    {
        static void SkillStart(Animal ani)
        {
            string str;
            str = Console.ReadLine();
            int a;

            if (int.TryParse(str, out a) && a >= 1 && a <= 3)
            {
                switch (a)
                {
                    case 1:
                        ani.Sleep();
                        break;
                    case 2:
                        ani.Eat();
                        break;
                    case 3:
                        ani.Cry();
                        break;
                }
            }
            else Console.WriteLine("잘못된 접근입니다");

        }
        static void Skill(Animal ani)
        {
            Console.WriteLine("1.휴식 2.먹이주기 3.포효");
            SkillStart(ani);
            return;
        }

        static void Main(string[] args)
        {
            Animal dog = new Dog();
            Console.WriteLine("태어난 개의 이름을 지어주세요");
            string str = Console.ReadLine();
            dog.Name = str;
            Console.WriteLine($"{dog.Name}");

            dog.GetStat();

            Console.WriteLine($"스킬을 사용하시겠습니까? (Y / N)");
            str = Console.ReadLine();
            if (str == "Y" || str == "y") Skill(dog);
            else if(str =="N" || str == "n") Console.WriteLine($"스킬 사용을 취소하셨습니다");
            else Console.WriteLine("잘못된 접근입니다");

            Console.WriteLine();

            Animal cat = new Cat();
            Console.WriteLine("태어난 고양이의 이름을 지어주세요");
            str = Console.ReadLine();
            cat.Name = str;
            Console.WriteLine($"{cat.Name}");

            cat.GetStat();

            Console.WriteLine($"스킬을 사용하시겠습니까? (Y / N)");
            str = Console.ReadLine();
            if (str == "Y" || str == "y") Skill(dog);
            else if (str == "N" || str == "n") Console.WriteLine($"스킬 사용을 취소하셨습니다");
            else Console.WriteLine("잘못된 접근입니다");

            Console.WriteLine();
        }
    }
}
