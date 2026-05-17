using System;
using System.Collections.Generic;
using Staff;

namespace Service
{
    public class EmployeeService
    {
        private readonly IMessageService _messageService;

        public EmployeeService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void ManageStaff(List<User> users)
        {
            while (true)
            {
                _messageService.ShowMessage("\n=== УПРАВЛІННЯ ПЕРСОНАЛОМ ===");
                _messageService.ShowMessage("1. Переглянути всіх співробітників");
                _messageService.ShowMessage("2. Додати нового співробітника");
                _messageService.ShowMessage("3. Видалити співробітника за логіном");
                _messageService.ShowMessage("0. Повернутися в головне меню");
                _messageService.ShowMessage("=============================");
                _messageService.ShowMessage("Оберіть дію:");

                string choice = Console.ReadLine()?.Trim() ?? "";

                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        ShowAllStaff(users);
                        break;
                    case "2":
                        AddStaffMember(users);
                        break;
                    case "3":
                        DeleteStaffMember(users);
                        break;
                    default:
                        _messageService.ShowMessage("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        private void ShowAllStaff(List<User> users)
        {
            _messageService.ShowMessage("\n--- Список співробітників аеропорту ---");
            int count = 0;

            foreach (var u in users)
            {
                if (u is not Client && u.Role != "Client")
                {
                    _messageService.ShowMessage($"ID: {u.Id} | Роль: {u.Role,-12} | Логін: {u.Login,-10} | ПІБ: {u.LastName} {u.Name} {u.ThirdName}");
                    count++;
                }
            }

            if (count == 0)
            {
                _messageService.ShowMessage("Наразі в системі немає зареєстрованого персоналу.");
            }
            _messageService.ShowMessage("---------------------------------------");
        }

        private void AddStaffMember(List<User> users)
        {
            _messageService.ShowMessage("\n--- Реєстрація нового співробітника ---");

            string login = "";
            while (true)
            {
                _messageService.ShowMessage("Введіть унікальний логін:");
                login = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(login))
                {
                    _messageService.ShowMessage("Логін не може бути порожнім.");
                    continue;
                }


                if (users.Exists(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                {
                    _messageService.ShowMessage("Цей логін уже зайнятий. Спробуйте інший.");
                    continue;
                }
                break;
            }

            _messageService.ShowMessage("Введіть пароль:");
            string password = Console.ReadLine()?.Trim() ?? "";

            _messageService.ShowMessage("Введіть ім'я:");
            string name = Console.ReadLine()?.Trim() ?? "";

            _messageService.ShowMessage("Введіть прізвище:");
            string lastName = Console.ReadLine()?.Trim() ?? "";

            _messageService.ShowMessage("Введіть по-батькові:");
            string thirdName = Console.ReadLine()?.Trim() ?? "";

            _messageService.ShowMessage("Введіть роль:");
            string role = Console.ReadLine()?.Trim() ?? "";


            User newEmployee = UserFactory.CreateUser(role, login, password, name, lastName, thirdName);

            users.Add(newEmployee);
            _messageService.ShowMessage($"\nСпівробітника [{login}] успішно створено з роллю {role}!");
        }


        private void DeleteStaffMember(List<User> users)
        {
            _messageService.ShowMessage("\n--- Видалення співробітника ---");
            _messageService.ShowMessage("Введіть логін особи, яку потрібно видалити:");
            string login = Console.ReadLine()?.Trim() ?? "";


            User employee = users.Find(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase))!;

            if (employee == null)
            {
                _messageService.ShowMessage("Користувача з таким логіном не знайдено.");
                return;
            }


            if (login.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                _messageService.ShowMessage("Помилка безпеки: Не можна видалити головного адміністратора системи!");
                return;
            }

            _messageService.ShowMessage($"Ви впевнені, що хочете видалити {employee.Role} [{login}] ({employee.Name} {employee.LastName})? (т/н):");
            string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (confirm == "т" || confirm == "y" || confirm == "yes")
            {

                users.Remove(employee);
                _messageService.ShowMessage($"Співробітника з логіном [{login}] успішно видалено з бази даних.");
            }
            else
            {
                _messageService.ShowMessage("Видалення скасовано.");
            }
        }

    }
}