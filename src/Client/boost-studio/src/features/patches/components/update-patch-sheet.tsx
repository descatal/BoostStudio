import React from "react"
import { PatchFileSummaryVm } from "@/api/exvs"
import { useUpdateTblPatchFile } from "@/features/patches/api/update-tbl-patch-file"
import { PatchFileForm } from "@/features/patches/components/patch-file-form"
import {
  PatchIdNameMap,
  useCustomizePatchInformationStore,
} from "@/pages/patches/libs/store"
import { zodResolver } from "@hookform/resolvers/zod"
import { Loader } from "lucide-react"
import { useForm } from "react-hook-form"

import { AlertDialogTrigger } from "@/components/ui/alert-dialog"
import { Button } from "@/components/ui/button"
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet"

import {
  CreatePatchFileSchema,
  createPatchFileSchema,
  UpdatePatchFileSchema,
  updatePatchFileSchema,
} from "../libs/validations"

interface UpdatePatchSheetProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  patchFile: PatchFileSummaryVm
  triggerButton?: React.ReactElement | undefined
}

const UpdatePatchSheet = ({
  triggerButton,
  patchFile,
  ...props
}: UpdatePatchSheetProps) => {
  const { selectedPatchFileVersion } = useCustomizePatchInformationStore(
    (state) => state
  )

  const patchName = PatchIdNameMap[selectedPatchFileVersion]

  const updateTblPatchFileMutation = useUpdateTblPatchFile({
    mutationConfig: {
      onSuccess: () => {},
    },
  })

  const form = useForm<CreatePatchFileSchema | UpdatePatchFileSchema>({
    resolver: patchFile
      ? zodResolver(updatePatchFileSchema)
      : zodResolver(createPatchFileSchema),
    defaultValues: patchFile
      ? patchFile
      : {
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
          assetFileHash: 0,
        },
  })

  return (
    <Sheet {...props}>
      <SheetTrigger asChild>{triggerButton ?? <></>}</SheetTrigger>
      <SheetContent className="flex flex-col gap-6 sm:max-w-md">
        <SheetHeader className="text-left">
          <SheetTitle>Update patch file details</SheetTitle>
          <SheetDescription>
            Update the patch file details and save the changes
          </SheetDescription>
          <PatchFileForm
            form={form}
            onSubmit={(values) => {
              if (patchFile) {
                updateTblPatchFileMutation.mutate({
                  ...values,
                  id: patchFile.id!,
                })
              }
            }}
          >
            <SheetFooter className="gap-2 pt-2 sm:space-x-0">
              <SheetClose asChild>
                <Button type="button" variant="outline">
                  Cancel
                </Button>
              </SheetClose>
              <Button disabled={updateTblPatchFileMutation.isPending}>
                {updateTblPatchFileMutation.isPending && (
                  <Loader
                    className="mr-2 size-4 animate-spin"
                    aria-hidden="true"
                  />
                )}
                Save
              </Button>
            </SheetFooter>
          </PatchFileForm>
        </SheetHeader>
      </SheetContent>
    </Sheet>
  )
}

export default UpdatePatchSheet
