using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{

    public class PlayerManager : IScene
    {
        Player player = new Player();

        public static void LevelCheck(Player player, Monster monster)
        {
            //몬스터가 죽으면 몬스터 레벨만큼 경험치 받기
            //겅혐치통 공식 (5 * (level * (level - 1)) / 2) + 10
            // 레벨 업하면 player.atk , def 증가
            player.exp += monster.level;

            if (player.exp > 5 * (player.level * (player.level - 1)) / 2) +10 ) {
                player.exp = 0;
                player.level += 1;
                player.atk += 0.5f; ;
                player.def += 1;

            }
        }

        public void Enter()
        {
            string input = Console.ReadLine();
            //캐릭터 이름 정하기
            player.name = input;


            //직업정하기

            int result = UtilManager.PlayerInput(1, 4);
            switch (result)
            {
                case 1: player.job = "전사";
                    player.maxhp += 30 ;
                    player.hp += player.maxhp;

                    break;
                case 2:
                    player.job = "마법사";
                    player.maxhp -= 10;
                    player.hp += player.maxhp;
                    player.atk += 7;

                    break;
                case 3:
                    player.job = "도적";
                    
                    
                    player.atk += 5;
                    break;
                case 4:
                    player.job = "궁수";

                    player.maxhp -= 20;
                    player.hp += player.maxhp;
                    player.atk += 10;
                    break;
                default:
                    break;
            }


            }


        }
    }

