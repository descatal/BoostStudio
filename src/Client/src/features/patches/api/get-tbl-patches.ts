import { tblApi } from "@/api/api";
import { GetApiPatchFilesSummaryRequest } from "@/api/exvs";
import { queryOptions, useQuery } from "@tanstack/react-query";
import { QueryConfig } from "@/lib/react-query";

function getPatchFilesSummary(options: GetApiPatchFilesSummaryRequest) {
  return tblApi.getApiPatchFilesSummary(options);
}

export const getPatchFilesSummaryOptions = (
  request: GetApiPatchFilesSummaryRequest,
) => {
  return queryOptions({
    queryKey: ["getPatchFilesSummary", request],
    queryFn: () => getPatchFilesSummary(request),
  });
};

type UseTblByIdOptions = {
  queryConfig?: QueryConfig<typeof getPatchFilesSummary>;
} & GetApiPatchFilesSummaryRequest;

export const useTblPatchFiles = ({
  queryConfig,
  ...options
}: UseTblByIdOptions = {}) => {
  return useQuery({
    ...getPatchFilesSummaryOptions(options),
    ...queryConfig,
  });
};
