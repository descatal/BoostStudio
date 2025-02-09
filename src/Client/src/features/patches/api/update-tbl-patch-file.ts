import { tblApi } from "@/features/api"
import { UpdatePatchFileSchema } from "@/features/patches/libs/validations"
import { useMutation } from "@tanstack/react-query"

import { MutationConfig } from "@/lib/react-query"

export const updateTblPatchFile = (request: UpdatePatchFileSchema) => {
  return tblApi.postApiPatchFilesById({
    id: request.id,
    updatePatchFileByIdCommand: request,
  })
}

type UseUpdateTblPatchFileOptions = {
  mutationConfig?: MutationConfig<typeof updateTblPatchFile>
}

export const useUpdateTblPatchFile = ({
  mutationConfig,
}: UseUpdateTblPatchFileOptions) => {
  const { onSuccess, ...restConfig } = mutationConfig || {}

  return useMutation({
    onSuccess: (...args) => {
      onSuccess?.(...args)
    },
    ...restConfig,
    mutationFn: updateTblPatchFile,
  })
}
