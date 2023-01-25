using OpenCover.Framework.Model;
using UnityEngine;

//================ файл для тестов вских штук =============================
namespace DefaultNamespace
{
    public class TestScript : MonoBehaviour
    {
        //------------------- образцовые REF --------------------
        private void Awake()
        {
            int x = 10;
            int y = 20;
            int sum = 0;
            Debug.Log($"- {x} - {y} - {sum}");
            
            SumAndRet( ref x, ref y, ref sum); // вызывает отсюда!! 
            Debug.Log($"- {x} - {y} - {sum}");
            // из-за ref переменные обновятся относительно первого заданного состояния
        }
        
        private void SumAndRet(ref int x, ref int y, ref int sum)
                 // у АК Ref только sum
                 {
                     // внутри делает что-то с переменными, 
                     x = 2 * x;
                     y = 2 * y;
                     sum = x + y;
                 }
        //--------------------------------------------------------

        
        //========= НАСЛЕДОВАНИЕ =================================
        public class MyClassBase
        {
            public int IntBase;

            //конструктор
            public MyClassBase()
            {
                Init();
            }

            private void Init()
            {
                MyClassA thisclassA = new MyClassA();
                thisclassA.IntBase = 10;
                thisclassA.IntA = 20;

                MyClassFromA thisclassfromA = new MyClassFromA();
                thisclassfromA.IntBase = 100;
                thisclassfromA.IntA2 = 200;
                thisclassfromA.IntA = 300;
                DoSomething(thisclassA);

                MyClassBase thisBaseClass = new MyClassBase();
                thisBaseClass.IntBase = 1;

                // Родители знают только о себе,
                // Дети знают о родителях

                // если к базовому классу присвоить экземплляр детей, то получим экзепляр базового класса
                MyClassBase classFromChild = new MyClassA();
                classFromChild.IntBase = 1;

                //------- присваиваем всех в базовый класс
                MyClassBase base1 = new MyClassBase();
                DoSomething(base1);

                MyClassBase base2 = new MyClassA();
                DoSomething(base2);

                MyClassBase base3 = new MyClassFromA();
                DoSomething(base3);
            }

            //=========== AS приводит к другому типу = "КАСТ" =============
            public void DoSomething(MyClassBase thisBase)
            {
                // небезопасно будет инксепшен и вылетит юнити О_о
                // MyClassA classA = thisBase as MyClassA; --> будет null
                // MyClassFromA classFromA = thisBase as MyClassFromA; -> будет null и надо проверку что не налл
                //
                //
            }



            public class MyClassA : MyClassBase
            {
                public int IntA;
            }

            public class MyClassB : MyClassBase
            {
                public int IntB;
            }

            public class MyClassFromA : MyClassA
            {
                public int IntA2;
            }

            public class MyClassFromB : MyClassB
            {
                public int IntB2;
            }
        }
    }
}