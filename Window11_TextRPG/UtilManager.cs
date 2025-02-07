using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window_11_TEXTRPG
{
    internal static class UtilManager
    {
        // 전반적으로 공통 되는 부분을 작성하는 static 클래스 

        // 플레이어 input
        public static int PlayerInput(int min, int max)
        {
            int input;
            while (true)
            {
                Console.Write("숫자를 입력하세요 >>> ");
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    break;  // 성공적으로 숫자를 입력받으면 반복문 종료
                }
                Console.WriteLine("올바른 숫자를 입력해주세요.");
            }

            return input;
        }

        // ##TODO : Iscene을 클래스로 만든 뒤 , 목록 list에 대한 리스트를 만든다
        // ex) string[] array = new string[4]{"1.플레이어정보","2.상점","3.인벤토리","4.저장"}
        // playerinput에 0 , array.length()를 넣으면 편할듯?
        // 추가로
        // disPlayermanager에서도 배열만 받으면
        // 배열의 길이만큼 돌면서 출력하면되서 편할듯
        // !!!!좀더 생각필요!

    }
}
