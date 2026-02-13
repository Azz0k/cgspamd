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


type ActionConfirmationDialogProps = {
  onCancel: ()=>void;
  onConfirm: () => Promise<boolean>;
  error: string | null;
  menuItemText :string;
  loading: boolean;
  description:string;
  title:string;
  children?: React.ReactNode;
}
export const ActionConfirmationDialog = ({
  onCancel,
  onConfirm,
  error,
  menuItemText,
  loading,
  description,
  title,
  children,
                                         }:ActionConfirmationDialogProps)=> {
  const [open, setOpen] = React.useState(false);
  const errorTitle = "Ошибка";
  const handleOpenChange = (value:boolean) => {

    setOpen(value);
    if (!value) onCancel();
  }
  const handleSubmit = () =>{
    onConfirm().then(result => {
      if (result) {
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
          {children}
          <DialogFooter>
            <Button
              type="submit"
              disabled={loading}
              onClick={handleSubmit}
            >
              {loading && <Spinner data-icon="inline-start" />}
              Да
            </Button>
            <DialogClose asChild>
              <Button variant="outline">Нет</Button>
            </DialogClose>
          </DialogFooter>
        </DialogContent>
      </form>
    </Dialog>
  )
}
