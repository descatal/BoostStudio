import React from "react";
import { Info } from "lucide-react";
import {
  Credenza,
  CredenzaBody,
  CredenzaClose,
  CredenzaContent,
  CredenzaFooter,
  CredenzaHeader,
  CredenzaTitle,
  CredenzaTrigger,
} from "@/components/credenza";
import { LuCircleAlert } from "react-icons/lu";
import { Separator } from "@/components/ui/separator";

interface ConfirmationDialogProps
  extends Omit<React.ComponentPropsWithRef<typeof Credenza>, "children"> {
  triggerButton: React.ReactElement;
  cancelButton?: React.ReactElement;
  confirmButton?: React.ReactElement;
  title: string;
  body?: string;
  icon?: "danger" | "info";
}

const ConfirmationDialog = ({
  triggerButton,
  cancelButton,
  confirmButton,
  title,
  body = "",
  icon = "danger",
  ...props
}: ConfirmationDialogProps) => {
  return (
    <Credenza {...props}>
      <CredenzaTrigger asChild>{triggerButton}</CredenzaTrigger>
      <CredenzaContent>
        <CredenzaHeader className="flex">
          <CredenzaTitle className="flex items-center gap-2">
            {icon === "danger" && (
              <LuCircleAlert className="size-6" aria-hidden="true" />
            )}
            {icon === "info" && <Info className="size-6" aria-hidden="true" />}
            {title}
          </CredenzaTitle>
        </CredenzaHeader>
        <CredenzaBody>
          <div className="grid gap-4">
            {body && (
              <div className="my-2">
                <p className={"text-sm text-muted-foreground"}>{body}</p>
              </div>
            )}
          </div>
          <Separator />
        </CredenzaBody>
        <CredenzaFooter>
          <CredenzaClose asChild>{cancelButton}</CredenzaClose>
          {confirmButton}
        </CredenzaFooter>
      </CredenzaContent>
    </Credenza>
  );
};

export default ConfirmationDialog;
