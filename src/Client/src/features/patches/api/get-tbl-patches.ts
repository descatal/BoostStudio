import { tblApi } from "@/api/api";
import { GetApiPatchFilesSummaryRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";

export const useTblPatchFiles = (options: GetApiPatchFilesSummaryRequest) => {
  return useQuery({
    queryKey: ["getApiPatchFilesSummary", options],
    queryFn: () => tblApi.getApiPatchFilesSummary(options),
    placeholderData: keepPreviousData,
  });
};
