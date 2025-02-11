// import React from "react"
// import { createPatchFiles, updatePatchFiles } from "@/api/wrapper/tbl-api"
// import { PatchFileForm } from "@/features/patches/components/patch-file-form"
// import {
//   PatchIdNameMap,
//   useCustomizePatchInformationStore,
// } from "@/pages/patches/libs/store"
// import { zodResolver } from "@hookform/resolvers/zod"
// import { ReloadIcon } from "@radix-ui/react-icons"
// import { useForm } from "react-hook-form"
//
// import { Button } from "@/components/ui/button"
// import {
//   Dialog,
//   DialogClose,
//   DialogContent,
//   DialogDescription,
//   DialogFooter,
//   DialogHeader,
//   DialogTitle,
//   DialogTrigger,
// } from "@/components/ui/dialog"
// import { toast } from "@/components/ui/use-toast"
//
// import {
//   CreatePatchFileSchema,
//   createPatchFileSchema,
//   updatePatchFileSchema,
//   UpdatePatchFileSchema,
// } from "../../libs/validations"
//
// interface PatchFileDialogProps
//   extends React.ComponentPropsWithoutRef<typeof Dialog> {
//   children?: React.ReactNode | undefined
//   existingPatchFile?: UpdatePatchFileSchema | undefined
//   onSuccess?: () => void
// }
//
// const PatchFileDialog = ({
//   children,
//   existingPatchFile,
//   onSuccess,
//   ...props
// }: PatchFileDialogProps) => {
//   const [isCreatePending, startCreateTransition] = React.useTransition()
//   // const isDesktop = useMediaQuery("(min-width: 640px)")
//
//   const { selectedPatchFileVersion } = useCustomizePatchInformationStore(
//     (state) => state
//   )
//
//   const patchName = PatchIdNameMap[selectedPatchFileVersion]
//
//   const form = useForm<CreatePatchFileSchema | UpdatePatchFileSchema>({
//     resolver: existingPatchFile
//       ? zodResolver(updatePatchFileSchema)
//       : zodResolver(createPatchFileSchema),
//     defaultValues: existingPatchFile
//       ? existingPatchFile
//       : {
//           tblId:
//             selectedPatchFileVersion === "All"
//               ? undefined
//               : selectedPatchFileVersion,
//           fileInfo: {
//             version:
//               selectedPatchFileVersion === "All"
//                 ? undefined
//                 : selectedPatchFileVersion,
//             size1: 0,
//             size2: 0,
//             size3: 0,
//             size4: 0,
//           },
//           pathInfo: {
//             path: `${patchName ?? ""}/`,
//             order: undefined,
//           },
//           assetFileHash: 0,
//         },
//   })
//
//   function onSubmit(input: CreatePatchFileSchema | UpdatePatchFileSchema) {
//     startCreateTransition(async () => {
//       if (updatePatchFileSchema.safeParse(input).success) {
//         const casted = input as UpdatePatchFileSchema
//         await updatePatchFiles({
//           id: casted.id,
//           updatePatchFileByIdCommand: {
//             id: casted.id,
//             tblId: casted.tblId,
//             fileInfo: casted.fileInfo,
//             pathInfo: casted.pathInfo,
//             assetFileHash: casted.assetFileHash,
//           },
//         })
//       } else if (createPatchFileSchema.safeParse(input).success) {
//         // input is of type UpdatePatchFileSchema
//         await createPatchFiles({
//           createPatchFileCommand: {
//             tblId: input.tblId,
//             fileInfo: input.fileInfo,
//             pathInfo: input.pathInfo,
//             assetFileHash: input.assetFileHash,
//           },
//         })
//       } else {
//         // input is neither of the two types
//         toast({
//           title: "Failed!",
//           description: "Unknown error occured!",
//         })
//       }
//
//       props.onOpenChange?.(false)
//       toast({
//         title: "Success!",
//         description: `Successfully ${existingPatchFile ? "updated" : "created"} a patch file entry.`,
//       })
//
//       if (onSuccess) onSuccess()
//     })
//   }
//
//   React.useEffect(() => {
//     if (existingPatchFile) {
//       form.reset(existingPatchFile)
//     }
//   }, [existingPatchFile])
//
//   return (
//     <Dialog {...props}>
//       {children && <DialogTrigger asChild>{children}</DialogTrigger>}
//       <DialogContent className={"max-h-screen"}>
//         <DialogHeader>
//           <DialogTitle>
//             {existingPatchFile ? "Update" : "Create"} patch file entry
//           </DialogTitle>
//           <DialogDescription>
//             Fill in the details below to{" "}
//             {existingPatchFile ? "update" : "create"} a new patch file entry.
//           </DialogDescription>
//         </DialogHeader>
//         <PatchFileForm form={form} onSubmit={onSubmit}>
//           <DialogFooter className="gap-2 pt-2 sm:space-x-0">
//             <DialogClose asChild>
//               <Button type="button" variant="outline">
//                 Cancel
//               </Button>
//             </DialogClose>
//             <Button disabled={isCreatePending}>
//               {isCreatePending && (
//                 <ReloadIcon
//                   className="size-4 mr-2 animate-spin"
//                   aria-hidden="true"
//                 />
//               )}
//               {existingPatchFile ? "Update" : "Create"}
//             </Button>
//           </DialogFooter>
//         </PatchFileForm>
//       </DialogContent>
//     </Dialog>
//   )
// }
//
// export default PatchFileDialog
