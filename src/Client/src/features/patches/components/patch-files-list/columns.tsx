"use client";

import React from "react";
import { type PatchFileSummaryVm } from "@/api/exvs";
import { ColumnDef } from "@tanstack/react-table";

import { Badge } from "@/components/ui/badge";
import { HashInput } from "@/components/custom/hash-input";
import PatchFilesListRowActions from "@/features/patches/components/patch-files-list/row-actions";

export const patchFilesListColumns: ColumnDef<PatchFileSummaryVm>[] = [
  {
    accessorKey: "assetFileHash",
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
  },
  {
    accessorKey: "fileType",
    header: "File Type",
    cell: ({ row }) => {
      return (
        <Badge className={"justify-center"}>
          {row.original.assetFile?.fileType ?? "Not Valid"}
        </Badge>
      );
    },
  },
  {
    accessorKey: "units",
    header: "Units",
    cell: ({ row }) => {
      if (
        !row.original.assetFile?.units ||
        row.original.assetFile?.units.length === 0
      )
        return (
          <Badge className={"w-fit"} variant={"outline"}>
            -
          </Badge>
        );

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
  },
  {
    accessorKey: "filePath",
    header: "File Path",
    cell: ({ row }) => (
      <div className="text-wrap">{row.original.pathInfo?.path}</div>
    ),
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
