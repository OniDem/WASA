using System;
using System.Windows;
using System.Windows.Controls;

namespace WASA.Сomplementary
{
    internal class InputCheck
    {
        private bool succeful = true;
        private char[] arr_false = new char[]
        {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ы', 'ъ', 'э', 'ю', 'я',
        'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ', 'Э', 'Ю', 'Я',
        '!', '@', '#', '$', '%', '&', '?', '-', '+', '=', '~'
        };

        public bool InputMultyplyCheck(TextBox atricle, TextBox price, TextBox count, TextBox discount, TextBox delete_id)
        {
            if (InCheck(atricle) && InCheck(price) && InCheck(count) && InCheck(discount) && InCheck(delete_id) == true)
                return true;
            else
                return false;
        }

        public bool InCheck(TextBox inputTextBocx)
        {
            try
            {
                string input = inputTextBocx.Text;
                for (int i = 0; i < input.Length; i++)
                {
                    for (int j = 0; j < arr_false.Length; j++)
                    {
                        if (input[i] == arr_false[j])
                        {
                            succeful = false;
                            break;
                        }
                        else
                        {
                            succeful = true;
                        }
                        if (succeful == false)
                        {
                            break;
                        }
                        else
                        {
                            succeful = true;
                        }
                    }
                    if (succeful == false)
                    {
                        break;
                    }
                    else
                    {
                        succeful = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return succeful;
        }
    }
}
