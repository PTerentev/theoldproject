using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace VoidInFateSite
{
    enum Roles
    {
        DefaultUser,
        Admin,
        Moderator
    }
    public class EmailValidator
    {

        public bool isValid(char[] input)
        {
            if (input == null)
            {
                return false;
            }

            int state = 0;
            char ch;
            int index = 0;
            int mark = 0;
            string local = null;
            List<string> domain = new List<string>();

            while (index <= input.Length && state != -1)
            {

                if (index == input.Length)
                {
                    ch = '\0'; // Так мы обозначаем конец нашей работы
                }
                else
                {
                    ch = input[index];
                    if (ch == '\0')
                    {
                        // символ, которым мы кодируем конец работы, не может быть частью ввода
                        return false;
                    }
                }

                switch (state)
                {

                    case 0:
                        {
                            // Первый символ {atext} -- текстовой части локального имени
                            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')
                                    || (ch >= '0' && ch <= '9') || ch == '_' || ch == '-'
                                    || ch == '+')
                            {
                                state = 1;
                                break;
                            }
                            // Если встретили неправильный символ -> отмечаемся в state об ошибке
                            state = -1;
                            break;
                        }

                    case 1:
                        {
                            // Остальные символы {atext} -- текстовой части локального имени
                            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')
                                    || (ch >= '0' && ch <= '9') || ch == '_' || ch == '-'
                                    || ch == '+')
                            {
                                break;
                            }
                            if (ch == '.')
                            {
                                state = 2;
                                break;
                            }
                            if (ch == '@')
                            { // Конец локальной части
                                local = new string(input, 0, index - mark);
                                mark = index + 1;
                                state = 3;
                                break;
                            }
                            // Если встретили неправильный символ -> отмечаемся в state об ошибке
                            state = -1;
                            break;
                        }

                    case 2:
                        {
                            // Переход к {atext} (текстовой части) после точки
                            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')
                                    || (ch >= '0' && ch <= '9') || ch == '_' || ch == '-'
                                    || ch == '+')
                            {
                                state = 1;
                                break;
                            }
                            // Если встретили неправильный символ -> отмечаемся в state об ошибке
                            state = -1;
                            break;
                        }

                    case 3:
                        {
                            // Переходим {alnum} (домену), проверяем первый символ
                            if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9')
                                    || (ch >= 'A' && ch <= 'Z'))
                            {
                                state = 4;
                                break;
                            }
                            // Если встретили неправильный символ -> отмечаемся в state об ошибке
                            state = -1;
                            break;
                        }

                    case 4:
                        {
                            // Собираем {alnum} --- домен
                            if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9')
                                    || (ch >= 'A' && ch <= 'Z'))
                            {
                                break;
                            }
                            if (ch == '-')
                            {
                                state = 5;
                                break;
                            }
                            if (ch == '.')
                            {
                                domain.Add(new string(input, mark, index - mark));
                                mark = index + 1;
                                state = 5;
                                break;
                            }
                            // Проверка на конец строки
                            if (ch == '\0')
                            {
                                domain.Add(new string(input, mark, index - mark));
                                state = 6;
                                break; // Дошли до конца строки -> заканчиваем работу
                            }
                            // Если встретили неправильный символ -> отмечаемся в state об ошибке
                            state = -1;
                            break;
                        }

                    case 5:
                        {
                            if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9')
                                    || (ch >= 'A' && ch <= 'Z'))
                            {
                                state = 4;
                                break;
                            }
                            if (ch == '-')
                            {
                                break;
                            }
                            // Если встретили неправильный символ -> отмечаемся в state об ошибке
                            state = -1;
                            break;
                        }

                    case 6:
                        {
                            // Успех! (На самом деле, мы сюда никогда не попадём)
                            break;
                        }
                }
                index++;
            }

            // Остальные проверки

            // Не прошли проверку выше? Возвращаем false!
            if (state != 6)
                return false;

            // Нам нужен домен как минимум второго уровня
            if (domain.Count() < 2)
                return false;

            // Ограничения длины по спецификации RFC 5321
            if (local.Length > 64)
                return false;

            // Ограничения длины по спецификации RFC 5321
            if (input.Length > 254)
                return false;

            // Домен верхнего уровня должен состоять только из букв и быть не короче двух символов
            index = input.Length - 1;
            while (index > 0)
            {
                ch = input[index];
                if (ch == '.' && input.Length - index > 2)
                {
                    return true;
                }
                if (!((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')))
                {
                    return false;
                }
                index--;
            }

            return true;
        }
    }
    public class Check
    {
        


            private string line2 { get; } =
            @"\W";
        private string line3 { get; } =
            @"[^0-9a-z]";
        private string line4 { get; } =
            @"\W";
        private string line5{ get; } =
            @"\W";

        private string line6 { get; } =
            @"[^0-9a-z]";
        
        public bool CheckingPassword(string str)
        {
            if (str.Length < 30)
            {
                var regex = new Regex(line5);
                if (regex.IsMatch(str) == false)
                {
                    var regex2 = new Regex(line6, RegexOptions.IgnoreCase);
                    if (regex2.IsMatch(str) == false)
                        return true;
                }
            }

            return false;
        }
        public bool CheckingName(string str)
        {
            if (str.Length < 20)
            {
                var regex = new Regex(line4);
                if (regex.IsMatch(str) == false)
                {

                    return true;
                }
            }

            return false;

        }
        public bool CheckingLogin(string str)
        {
            if (str.Length < 20)
            {
                var regex = new Regex(line2);
                if (regex.IsMatch(str) == false)
                {
                    var regex2 = new Regex(line3, RegexOptions.IgnoreCase);
                    if (regex2.IsMatch(str) == false)
                        return true;
                }
            }

            return false;


        }
        public bool CheckingMail(string str)
        {
            EmailValidator valid = new EmailValidator();
            
            return valid.isValid(str.ToCharArray());
        }
    }
}