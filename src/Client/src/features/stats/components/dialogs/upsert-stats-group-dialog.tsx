import React from "react";
import { CreateStatCommand, StatDto, UpdateStatCommand } from "@/api/exvs";
import { ChevronDown, PlusIcon } from "lucide-react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  getApiStatsOptions,
  getApiStatsQueryKey,
  postApiStatsByIdMutation,
  postApiStatsMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import { EnhancedButton } from "@/components/ui/enhanced-button.tsx";
import { BiSave, BiX } from "react-icons/bi";
import { Icons } from "@/components/icons.tsx";
import { Label } from "@/components/ui/label.tsx";
import { AutoForm } from "@/components/ui/autoform";
import { ZodProvider } from "@autoform/zod/v4";
import { zCreateStatCommand, zUpdateStatCommand } from "@/api/exvs/zod.gen.ts";
import { Separator } from "@/components/ui/separator.tsx";
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@/components/ui/drawer.tsx";
import { useMediaQuery } from "@/hooks/use-media-query";
import {
  Combobox,
  ComboboxAnchor,
  ComboboxContent,
  ComboboxEmpty,
  ComboboxGroup,
  ComboboxGroupLabel,
  ComboboxInput,
  ComboboxItem,
  ComboboxTrigger,
} from "@/components/ui/combobox";
import { toast } from "sonner";

interface Props
  extends Omit<React.ComponentPropsWithRef<typeof DrawerContent>, "children"> {
  existingData?: StatDto | undefined;
  unitId?: number | undefined;
  children?: React.ReactNode;
}

const UpsertStatsGroupDialog = ({
  existingData,
  children,
  unitId,
  ...props
}: Props) => {
  // put this into global state
  const [open, setOpen] = React.useState(false);
  const [data, setData] = React.useState(existingData);
  const [duplicateSelection, setDuplicateSelection] = React.useState("");
  const [duplicateInput, setDuplicateInput] = React.useState("");

  const queryClient = useQueryClient();
  const { data: duplicateOptions } = useQuery({
    ...getApiStatsOptions({
      query: {
        UnitIds: unitId
          ? [unitId]
          : existingData?.unitId
            ? [existingData.unitId]
            : undefined,
      },
    }),
    select: (data) => {
      return data.items.sort((a, b) => (a.order ?? 0) - (b.order ?? 0)) ?? [];
    },
  });

  const createMutation = useMutation({
    ...postApiStatsMutation(),
    onSuccess: onSuccess,
  });

  const updateMutation = useMutation({
    ...postApiStatsByIdMutation(),
    onSuccess: onSuccess,
  });

  const isPending = createMutation.isPending || updateMutation.isPending;

  async function onSuccess() {
    toast("Success", {
      description: `${existingData ? "Update" : "Create"} success!`,
    });

    setOpen(false);

    await queryClient.invalidateQueries({
      predicate: (query) =>
        // @ts-ignore
        query.queryKey[0]._id === getApiStatsQueryKey()[0]._id,
    });
  }

  const handleSubmit = (upsertData: CreateStatCommand | UpdateStatCommand) => {
    existingData
      ? updateMutation.mutate({
          path: {
            id: (upsertData as UpdateStatCommand).id,
          },
          body: upsertData as UpdateStatCommand,
        })
      : createMutation.mutate({ body: upsertData as CreateStatCommand });
  };

  const schemaProvider = new ZodProvider(
    existingData ? zUpdateStatCommand : zCreateStatCommand,
  );
  const isDesktop = useMediaQuery("(min-width: 768px)");

  React.useEffect(() => {
    const matchedData = duplicateOptions?.find(
      (option) => option.id === duplicateSelection,
    );
    if (matchedData) setData(matchedData);
  }, [duplicateSelection]);

  return (
    <Drawer
      open={open}
      onOpenChange={setOpen}
      direction={isDesktop ? "right" : "bottom"}
    >
      <DrawerTrigger asChild>
        {children ?? (
          <EnhancedButton
            variant="default"
            effect={"ringHover"}
            size={"sm"}
            icon={PlusIcon}
            iconPlacement={"right"}
          >
            Create
          </EnhancedButton>
        )}
      </DrawerTrigger>
      <DrawerContent {...props}>
        <DrawerHeader className="px-6 pt-6 pb-4 border-b">
          <div className={"flex flex-row items-center justify-between"}>
            <div className="space-y-4">
              <div className="flex items-center justify-between">
                <div className="space-y-1">
                  <DrawerTitle>
                    {existingData ? "Update" : "Create"} Stats
                  </DrawerTitle>
                  <DrawerDescription>
                    {existingData
                      ? `Update the unit stats information below.`
                      : "Fill out the unit stats information below."}
                  </DrawerDescription>
                </div>
              </div>
            </div>
          </div>
          {existingData && (
            <div className="flex items-center gap-2 px-3 py-2 bg-muted rounded-md">
              <span className="text-sm font-medium text-muted-foreground">
                ID:
              </span>
              <code className="text-sm font-mono bg-background px-2 py-1 rounded border">
                {existingData.id}
              </code>
            </div>
          )}
          <div className="flex items-center gap-2 w-full pt-2">
            <Combobox
              className={"w-full"}
              inputValue={duplicateInput}
              onInputValueChange={setDuplicateInput}
              value={duplicateSelection}
              onValueChange={setDuplicateSelection}
            >
              <ComboboxAnchor>
                <ComboboxInput placeholder="Duplicate from..." />
                <ComboboxTrigger>
                  <ChevronDown className="h-4 w-4" />
                </ComboboxTrigger>
              </ComboboxAnchor>
              <ComboboxContent>
                <ComboboxEmpty>No other stats group found</ComboboxEmpty>
                <ComboboxGroup>
                  <ComboboxGroupLabel>Stats</ComboboxGroupLabel>
                  {duplicateOptions?.map((option) => (
                    <ComboboxItem key={option.id!} value={option.id!}>
                      {option.order} ({option.id!})
                    </ComboboxItem>
                  ))}
                </ComboboxGroup>
              </ComboboxContent>
            </Combobox>
            <EnhancedButton
              className={"w-fit"}
              variant="outline"
              size="sm"
              type={"reset"}
              icon={BiX}
              effect={"expandIcon"}
              iconPlacement={"right"}
              form={"form"}
              onClick={() => {
                setDuplicateSelection("");
                setDuplicateInput("");
                setData(undefined);
              }}
            >
              Clear
            </EnhancedButton>
          </div>
        </DrawerHeader>
        <Separator />
        <AutoForm
          schema={schemaProvider}
          defaultValues={existingData}
          values={data}
          onSubmit={(submitData) => {
            handleSubmit({ ...submitData });
          }}
          formProps={{
            id: "form",
            className: "px-8 py-4 overflow-auto",
          }}
          uiComponents={{
            FieldWrapper: ({ children, label, error }) => (
              <div className={"py-2"}>
                <Label className={"pb-2"}>{label}</Label>
                {children}
                {error}
              </div>
            ),
          }}
        />
        <DrawerFooter>
          <EnhancedButton
            effect={"expandIcon"}
            variant={"default"}
            icon={BiSave}
            iconPlacement={"right"}
            disabled={isPending}
            form={"form"}
            type={"submit"}
          >
            {isPending && (
              <Icons.spinner
                className="size-4 mr-2 animate-spin"
                aria-hidden="true"
              />
            )}
            Save
          </EnhancedButton>
          <DrawerClose asChild>
            <EnhancedButton variant={"outline"} disabled={isPending}>
              Cancel
            </EnhancedButton>
          </DrawerClose>
        </DrawerFooter>
      </DrawerContent>
    </Drawer>
  );
};

export default UpsertStatsGroupDialog;
