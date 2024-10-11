import React from "react"
import { createPatchFiles } from "@/api/wrapper/tbl-api"
import {
  PatchIdNameMap,
  useCustomizePatchInformationStore,
} from "@/pages/patches/libs/store"
import { zodResolver } from "@hookform/resolvers/zod"
import { ReloadIcon } from "@radix-ui/react-icons"
import { PlusIcon } from "lucide-react"
import { useForm } from "react-hook-form"

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
import { toast } from "@/components/ui/use-toast"

import {
  CreatePatchFileSchema,
  createPatchFileSchema,
} from "../../libs/validations"
import { CreatePatchFileForm } from "./create-patch-file-form"

const CreatePatchFileDialog = () => {
  const [open, setOpen] = React.useState(false)
  const [isCreatePending, startCreateTransition] = React.useTransition()
  // const isDesktop = useMediaQuery("(min-width: 640px)")

  const { selectedPatchFileVersion } = useCustomizePatchInformationStore(
    (state) => state
  )

  const patchName = PatchIdNameMap[selectedPatchFileVersion]

  const form = useForm<CreatePatchFileSchema>({
    resolver: zodResolver(createPatchFileSchema),
    defaultValues: {
      tblId:
        selectedPatchFileVersion === "All"
          ? undefined
          : selectedPatchFileVersion,
      fileInfo: {
        version:
          selectedPatchFileVersion === "All"
            ? undefined
            : selectedPatchFileVersion,
        size1: 0,
        size2: 0,
        size3: 0,
        size4: 0,
      },
      pathInfo: {
        path: `${patchName ?? ""}/`,
        order: undefined,
      },
      assetFileHash: undefined,
    },
  })

  function onSubmit(input: CreatePatchFileSchema) {
    startCreateTransition(async () => {
      form.reset()

      await createPatchFiles({
        createPatchFileCommand: {
          tblId: input.tblId,
          fileInfo: input.fileInfo,
          pathInfo: input.pathInfo,
          assetFileHash: input.assetFileHash,
        },
      })

      setOpen(false)
      toast({
        title: "Success!",
        description: "Successfully created a new patch file entry.",
      })
    })
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button variant="outline" size="sm">
          <PlusIcon className="size-4 mr-2" aria-hidden="true" />
          New patch file entry
        </Button>
      </DialogTrigger>
      <DialogContent className={"max-h-screen"}>
        <DialogHeader>
          <DialogTitle>Create patch file entry</DialogTitle>
          <DialogDescription>
            Fill in the details below to create a new patch file entry.
          </DialogDescription>
        </DialogHeader>
        <CreatePatchFileForm form={form} onSubmit={onSubmit}>
          <DialogFooter className="gap-2 pt-2 sm:space-x-0">
            <DialogClose asChild>
              <Button type="button" variant="outline">
                Cancel
              </Button>
            </DialogClose>
            <Button disabled={isCreatePending}>
              {isCreatePending && (
                <ReloadIcon
                  className="size-4 mr-2 animate-spin"
                  aria-hidden="true"
                />
              )}
              Create
            </Button>
          </DialogFooter>
        </CreatePatchFileForm>
      </DialogContent>
    </Dialog>
  )
}

export default CreatePatchFileDialog
