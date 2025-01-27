import { type ExportTblCommand } from "@/api/exvs"
import { tblApi } from "@/features/api"
import { useMutation } from "@tanstack/react-query"

import { MutationConfig } from "@/lib/react-query"

export const exportTbl = (request: ExportTblCommand) => {
  return tblApi.postApiTblExport({
    exportTblCommand: request,
  })
}

type UseExportTblOptions = {
  mutationConfig?: MutationConfig<typeof exportTbl>
}

export const useExportTbl = ({ mutationConfig }: UseExportTblOptions) => {
  const { onSuccess, ...restConfig } = mutationConfig || {}

  return useMutation({
    onSuccess: (...args) => {
      onSuccess?.(...args)
    },
    ...restConfig,
    mutationFn: exportTbl,
  })
}
