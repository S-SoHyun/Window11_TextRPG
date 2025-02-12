using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window11_TextRPG;

namespace Window_11_TEXTRPG
{
    internal static class UtilManager
    {
        // 전반적으로 공통 되는 부분을 작성하는 static 클래스 

        // 플레이어 input
        public static int PlayerInput(int min, int max)
        {
            // 매개변수 :
            // min, max 포함되게 

            int input;
            while (true)
            {
                Console.Write($"{min} ~ {max} 숫자를 입력하세요 >>> ");
                if (int.TryParse(Console.ReadLine(), out input)
                    && input >= min 
                    && input <= max)
                {
                    // 성공적으로 숫자를 입력받으면 종료 
                    // 범위내에 입력하면 종료

                    break;  
                }
                Console.WriteLine("올바른 숫자를 입력해주세요.");
            }

            return input;
        }

        //소수점 올림
        public static int GetCeiling(double number)
        {
            return (int)Math.Ceiling(number);
        }

        //랜덤함수 min 이상 max 미만
        public static int getRandomInt(int min, int max) 
        {
            return new Random().Next(min, max);
        }


        // N초동안 기다리기
        public static void DelayForSecond(int second)
        {
            Thread.Sleep(second * 1000);   // ex) 3 * 1000 : 3초 대기
        }

        //데미지 계산
        public static int CalcDamage(int hp, int damage)
        {
            if (hp - damage < 0)
            {
                hp = 0;
            }
            else
            {
                hp -= damage;
            }
            return hp;
        }

        // 플레이어 체력 계산
        public static int CalcPlayerHp(Player player, int num)
        {
            int hp = player.hp + num >= player.maxhp ? player.maxhp : player.hp + num;
            return hp;
        }

    }
}
