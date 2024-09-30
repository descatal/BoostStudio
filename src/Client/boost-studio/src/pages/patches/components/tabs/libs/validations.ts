import {
  CreatePatchFileCommand,
  FileInfoDto,
  type PatchFileVersion,
  type PathInfoDto,
  type UpdatePatchFileByIdCommand,
} from "@/api/exvs"
import { z } from "zod"

export const updatePatchFileSchema = z.object({
  id: z.string(),
  tblId: z.custom<PatchFileVersion>(),
  pathInfo: z.custom<PathInfoDto>(),
  fileInfo: z.custom<FileInfoDto>(),
  assetFileHash: z.number().optional(),
}) satisfies z.ZodType<UpdatePatchFileByIdCommand>

export type UpdatePatchFileSchema = z.infer<typeof updatePatchFileSchema>

export const createPatchFileSchema = z.object({
  tblId: z.custom<PatchFileVersion>(),
  pathInfo: z.custom<PathInfoDto>().optional(),
  fileInfo: z.custom<FileInfoDto>().optional(),
  assetFileHash: z.number().optional(),
}) satisfies z.ZodType<CreatePatchFileCommand>

export type CreatePatchFileSchema = z.infer<typeof createPatchFileSchema>

export const pathInfo = z.object({
  path: z.string(),
  order: z.number(),
}) satisfies z.ZodType<PathInfoDto>

export const fileInfoDto = z.object({
  version: z.custom<PatchFileVersion>(),
  size1: z.number(),
  size2: z.number(),
  size3: z.number(),
  size4: z.number(),
}) satisfies z.ZodType<FileInfoDto>

export interface AssetFileSearch {
  assetFileHash: number
}

export const searchAssetFileSearchSchema = z.object({
  assetFileHash: z.number(),
}) satisfies z.ZodType<AssetFileSearch>

export type SearchAssetFileSchema = z.infer<typeof searchAssetFileSearchSchema>
