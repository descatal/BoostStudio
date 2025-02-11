import { tblApi } from "@/api/api"
import { type ResizePatchFileCommand } from "@/api/exvs"
import { useMutation } from "@tanstack/react-query"

import { MutationConfig } from "@/lib/react-query"

export const resizeTblPatches = (request: ResizePatchFileCommand) => {
  return tblApi.postApiPatchFilesResize({
    resizePatchFileCommand: request,
  })
}

type UseResizeTblPatchesOptions = {
  mutationConfig?: MutationConfig<typeof resizeTblPatches>
}

export const useResizeTblPatches = ({
  mutationConfig,
}: UseResizeTblPatchesOptions) => {
  const { onSuccess, ...restConfig } = mutationConfig || {}

  return useMutation({
    onSuccess: (...args) => {
      onSuccess?.(...args)
    },
    ...restConfig,
    mutationFn: resizeTblPatches,
  })
}
