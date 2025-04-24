import { tblApi } from "@/api/api";
import { GetApiTblByIdRequest } from "@/api/exvs";
import { queryOptions, useQuery } from "@tanstack/react-query";
import { QueryConfig } from "@/lib/react-query";

function getTblById(options: GetApiTblByIdRequest) {
  return tblApi.getApiTblById(options);
}

export const getTblByIdOptions = (request: GetApiTblByIdRequest) => {
  return queryOptions({
    queryKey: ["getTblById", request],
    queryFn: () => getTblById(request),
  });
};

type UseTblByIdOptions = {
  queryConfig?: QueryConfig<typeof getTblById>;
} & GetApiTblByIdRequest;

export const useTblById = ({ queryConfig, ...options }: UseTblByIdOptions) => {
  return useQuery({
    ...getTblByIdOptions(options),
    ...queryConfig,
  });
};
