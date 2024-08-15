import {z} from "zod";
import { PostApiConfigsRequest } from "@/api/exvs/models/PostApiConfigsRequest";

export const upsertConfigSchema = z.object({
  key: z.string(),
  value: z.string(),
}) satisfies z.ZodType<PostApiConfigsRequest>

export type UpsertConfigSchema = z.infer<typeof upsertConfigSchema>