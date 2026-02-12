import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import {DropdownMenuItem} from "@/components/ui/dropdown-menu.tsx";
import React from "react";
import {Spinner} from "@/components/ui/spinner.tsx";


type DeleteConfirmationDialogProps = {
  onCancel: ()=>void;
  onConfirm: () => Promise<boolean>;
  error: string | null;
  menuItemText :string;
  loading: boolean;
}
export const DeleteConfirmationDialog = ({
                                           onCancel,
                                           onConfirm,
                                           error,
                                           menuItemText,
  loading,
}:DeleteConfirmationDialogProps)=> {
  const [open, setOpen] = React.useState(false);
  const [disabled,setDisabled] = React.useState(false);
  const description = "Это действие нельзя отменить.";
  const title = "Вы уверены?";
  const errorTitle = "Ошибка";

  const handleOpenChange = (value:boolean) => {

    setOpen(value);
    if (!value) onCancel();
  }
  const handleSubmit = () =>{
    setDisabled(true);
    onConfirm().then(result => {
      if (result) {
        setDisabled(false);
        setOpen(false);
        onCancel();
      }
    })
  };
  const handleMenuItemSelected = (e:Event) => {
    e.preventDefault();
    setOpen(true);
  }
  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <form>
        <DialogTrigger asChild>
          <DropdownMenuItem onSelect={handleMenuItemSelected}>{menuItemText}</DropdownMenuItem>
        </DialogTrigger>
        <DialogContent className="sm:max-w-sm">
          <DialogHeader>
            <DialogTitle>{error===null?title:errorTitle}</DialogTitle>
            <DialogDescription>
              {error===null?description:error}
            </DialogDescription>
          </DialogHeader>
          <DialogFooter>
            {loading ? (
              <Button
                type="submit"
                disabled={disabled}
              >
                <Spinner data-icon="inline-start" />
                Да
              </Button>
            ):(
              <Button
                type="submit"
                disabled={disabled}
                onClick={handleSubmit}
              >Да</Button>
            )}
            <DialogClose asChild>
              <Button variant="outline">Нет</Button>
            </DialogClose>
          </DialogFooter>
        </DialogContent>
      </form>
    </Dialog>
  )
}
