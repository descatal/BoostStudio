import { toast } from "@/hooks/use-toast";
import { ProblemDetails } from "get-problem-details";

export async function GetProblemDetails(error: Error) {
  // @ts-ignore
  if (error?.response instanceof Response) {
    // @ts-ignore
    const response = error.response as Response;
    const responseResult = (await response.clone().json()) as unknown;

    // problem details payload, check RFC
    return new ProblemDetails(responseResult);
  }
}

export async function ShowErrorToast(error: Error) {
  const problemDetails = await GetProblemDetails(error);
  const errorMessage =
    problemDetails?.title ?? error?.message ?? "Unspecified error!";

  toast({
    title: "Error",
    description: errorMessage,
    variant: "destructive",
  });
}
