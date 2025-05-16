import { ProblemDetails } from "get-problem-details";
import { toast } from "sonner";

export async function GetProblemDetails(error: Error) {
  // @ts-ignore
  if (error?.response instanceof Response) {
    // @ts-ignore
    const response = error.response as Response;
    const responseResult = (await response.clone().json()) as unknown;

    // problem details payload, check RFC
    return new ProblemDetails(responseResult);
  } else {
    return new ProblemDetails(error);
  }
}

export async function ShowErrorToast(error: Error) {
  const problemDetails = await GetProblemDetails(error);
  const errorMessage =
    problemDetails?.title ??
    problemDetails?.detail ??
    error?.message ??
    "Unspecified error!";

  toast.error(errorMessage);
}
