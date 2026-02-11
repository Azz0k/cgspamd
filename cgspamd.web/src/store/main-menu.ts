import {MailX, UserCog} from "lucide-react";

export const mainMenu = {
  navAdmin: [
    {
      title: "Правила фильтрации",
      url: "/",
      icon: MailX,
      isActive: true,
    },
    {
      title: "Пользователи",
      url: "/users",
      icon: UserCog,
    }
  ],
  navMain: [
    {
      title: "Правила фильтрации",
      url: "/",
      icon: MailX,
      isActive: true,
    },
  ],
}