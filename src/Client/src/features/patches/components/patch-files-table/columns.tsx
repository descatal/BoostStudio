"use client";

import React from "react";
import { type PatchFileSummaryVm } from "@/api/exvs";
import { ColumnDef } from "@tanstack/react-table";

import { Badge } from "@/components/ui/badge";
import { HashInput } from "@/components/custom/hash-input";
import PatchFilesListRowActions from "@/features/patches/components/patch-files-table/row-actions";
import { zAssetFileType } from "@/api/exvs/zod.gen";

export const patchFilesListColumns: ColumnDef<PatchFileSummaryVm>[] = [
  {
    id: "assetFileHash",
    accessorFn: (row) => row.assetFile?.hash ?? row.assetFileHash,
    header: "File Hash",
    cell: ({ row }) => (
      <HashInput
        className={"border-none"}
        initialValue={
          row.original.assetFile?.hash ??
          row.original.assetFileHash ??
          undefined
        }
        readonly={true}
        initialMode={"hex"}
      />
    ),
    meta: {
      label: "Hash",
      variant: "text",
      placeholder: "Search by Hash (Hex)",
    },
    enableColumnFilter: true,
  },
  {
    id: "fileType",
    accessorFn: (row) => row.assetFile?.fileType,
    header: "File Type",
    cell: ({ row }) => {
      const fileTypes = row.original.assetFile?.fileType;
      if (!fileTypes || fileTypes.length === 0) {
        return (
          <Badge className={"w-fit"} variant={"outline"}>
            -
          </Badge>
        );
      }

      return (
        <div className={"flex flex-col gap-2"}>
          {fileTypes &&
            fileTypes.map((fileType, index) => (
              <Badge key={index} className={"justify-center"}>
                {fileType}
              </Badge>
            ))}
        </div>
      );
    },
    meta: {
      label: "File Type",
      variant: "multiSelect",
      options: Object.keys(zAssetFileType.Enum).map((type) => ({
        label: type,
        value: type,
      })),
    },
    enableColumnFilter: true,
  },
  {
    id: "unitId",
    accessorFn: (row) => row.assetFile?.units.map((x) => x.unitId),
    header: "Units",
    cell: ({ row }) => {
      if (
        !row.original.assetFile?.units ||
        row.original.assetFile?.units.length === 0
      ) {
        return (
          <Badge className={"w-fit"} variant={"outline"}>
            -
          </Badge>
        );
      }

      return (
        <div className={"flex flex-col gap-2"}>
          {row.original.assetFile.units.map((unit, i) => (
            <Badge
              className={"w-fit"}
              variant={"outline"}
              key={`${unit.unitId}-${i}`}
            >
              {unit.nameEnglish}
            </Badge>
          ))}
        </div>
      );
    },
    meta: {
      label: "Unit",
      variant: "multiSelect",
    },
    enableColumnFilter: true,
  },
  {
    id: "filePath",
    accessorFn: (row) => row.pathInfo?.path,
    header: "File Path",
  },
  {
    id: "actions",
    size: 40,
    cell: ({ row }) => {
      if (!row.original.id) return;
      return <PatchFilesListRowActions data={row.original} />;
    },
  },
];
