import React, { useEffect } from "react"
import { Info } from "lucide-react"

import { useDisclosure } from "@/hooks/use-disclosure"
import {
  AlertDialog,
  AlertDialogContent,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import { Button } from "@/components/ui/button"

export type ConfirmationDialogProps = {
  triggerButton: React.ReactElement
  confirmButton: React.ReactElement
  title: string
  body?: string
  cancelButtonText?: string
  icon?: "danger" | "info"
  isDone?: boolean
}

const ConfirmationDialog = ({
  triggerButton,
  confirmButton,
  title,
  body = "",
  cancelButtonText = "Cancel",
  icon = "danger",
  isDone = false,
}: ConfirmationDialogProps) => {
  const { close, open, isOpen } = useDisclosure()
  const cancelButtonRef = React.useRef(null)

  useEffect(() => {
    if (isDone) {
      close()
    }
  }, [isDone, close])

  return (
    <AlertDialog
      open={isOpen}
      onOpenChange={(isOpen) => {
        if (!isOpen) {
          close()
        } else {
          open()
        }
      }}
    >
      <AlertDialogTrigger asChild>{triggerButton}</AlertDialogTrigger>
      <AlertDialogContent className="sm:max-w-[425px]">
        <AlertDialogHeader className="flex">
          <AlertDialogTitle className="flex items-center gap-2">
            {" "}
            {icon === "danger" && (
              <Info className="size-6 text-red-600" aria-hidden="true" />
            )}
            {icon === "info" && (
              <Info className="size-6 text-blue-600" aria-hidden="true" />
            )}
            {title}
          </AlertDialogTitle>
        </AlertDialogHeader>

        <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
          {body && (
            <div className="mt-2">
              <p>{body}</p>
            </div>
          )}
        </div>

        <AlertDialogFooter>
          {confirmButton}
          <Button ref={cancelButtonRef} variant="outline" onClick={close}>
            {cancelButtonText}
          </Button>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}

export default ConfirmationDialog
