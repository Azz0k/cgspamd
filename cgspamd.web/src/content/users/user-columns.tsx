import  { type ColumnDef } from "@tanstack/react-table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {Button} from "@/components/ui/button.tsx";
import { MoreHorizontal } from "lucide-react"

export type User = {
  id: number;
  userName: string;
  fullName: string;
  isAdmin: boolean;
  enabled: boolean;
};

export const userColumns:ColumnDef<User>[] = [
  {
    accessorKey: "userName",
    header: "Логин",
  },
  {
    accessorKey: "fullName",
    header: "ФИО",
  },
  {
    accessorKey: "isAdmin",
    header: "Администратор",
  },
  {
    accessorKey: "enabled",
    header: "Включен",
  },
  {
    id: "actions",
    cell: ({ row }) => {
      const user = row.original
      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" className="h-8 w-8 p-0">
              <span className="sr-only">Open menu</span>
              <MoreHorizontal className="h-4 w-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Действия</DropdownMenuLabel>
            <DropdownMenuSeparator />
            <DropdownMenuItem>Добавить пользователя</DropdownMenuItem>
            <DropdownMenuItem>Редактировать пользователя</DropdownMenuItem>
            <DropdownMenuItem>Удалить пользователя</DropdownMenuItem>
            <DropdownMenuItem>Сменить пароль пользователю</DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      )
    },
  },
];

