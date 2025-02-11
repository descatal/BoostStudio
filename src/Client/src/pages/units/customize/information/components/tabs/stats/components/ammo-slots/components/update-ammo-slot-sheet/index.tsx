import React, { useEffect } from "react"
import { updateUnitAmmoSlot } from "@/api/wrapper/stats-api"
import { zodResolver } from "@hookform/resolvers/zod"
import { ReloadIcon } from "@radix-ui/react-icons"
import { useForm } from "react-hook-form"

import { Button } from "@/components/ui/button"
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet"
import { toast } from "@/components/ui/use-toast"

import {
  UpdateAmmoSlotSchema,
  updateAmmoSlotSchema,
} from "../../libs/validations"
import { AmmoSlotContext, AmmoSlotContextType } from "../../types"

interface UpdateAmmoSlotSheetProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  unitId: number
  index: number
  ammoOptions: number[]
}

const UpdateAmmoSlotSheet = ({
  children,
  unitId,
  index,
  ammoOptions,
  ...props
}: UpdateAmmoSlotSheetProps) => {
  const { ammoSlots, setAmmoSlots } =
    React.useContext<AmmoSlotContextType>(AmmoSlotContext)
  const ammoSlot = ammoSlots[index]
  const [isUpdatePending, startUpdateTransition] = React.useTransition()
  const [openBool, setOpenBool] = React.useState(false)

  const form = useForm<UpdateAmmoSlotSchema>({
    resolver: zodResolver(updateAmmoSlotSchema),
    defaultValues: {
      id: ammoSlot.id ?? "",
      ammoHash: ammoSlot.ammoHash,
      unitId: unitId,
      slotOrder: ammoSlot.slotOrder,
    },
  })

  useEffect(() => {
    form.setValue("id", ammoSlot.id ?? "")
    form.setValue("ammoHash", ammoSlot.ammoHash ?? 0)
    form.setValue("unitId", unitId)
    form.setValue("slotOrder", ammoSlot.slotOrder ?? 0)
  }, [ammoSlots])

  function onSubmit(input: UpdateAmmoSlotSchema) {
    // startUpdateTransition(async () => {
    //   await updateUnitAmmoSlot({
    //     id: input.id,
    //     updateUnitAmmoSlotCommand: input,
    //   })
    //
    //   ammoSlots[index] = {
    //     id: input.id,
    //     ammoHash: input.ammoHash,
    //     slotOrder: input.slotOrder,
    //   }
    //   setAmmoSlots(ammoSlots)
    //
    //   form.reset()
    //   setOpenBool(false)
    //   props.onOpenChange?.(false)
    //   toast({
    //     title: `Ammo Slot ${(ammoSlot.slotOrder ?? 0) + 1} Updated!`,
    //   })
    // })
  }

  return (
    <div>
      <Sheet {...props} open={openBool} onOpenChange={setOpenBool}>
        <SheetTrigger asChild>{children}</SheetTrigger>
        <SheetContent className="flex flex-col gap-6 sm:max-w-md">
          <SheetHeader className="text-left">
            <SheetTitle>Ammo Slot {(ammoSlot.slotOrder ?? 0) + 1}</SheetTitle>
            <SheetDescription>
              Select the ammo to be assigned to the ammo slot when spawned.
            </SheetDescription>
          </SheetHeader>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
              <FormField
                control={form.control}
                name="ammoHash"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Ammo Hash</FormLabel>
                    <FormControl>
                      <Select
                        onValueChange={field.onChange}
                        defaultValue={field.value?.toString()}
                      >
                        <FormControl>
                          <SelectTrigger className="capitalize">
                            <SelectValue placeholder="Select ammo" />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent>
                          <SelectGroup>
                            {ammoOptions.map((item) => (
                              <SelectItem
                                key={item}
                                value={item.toString()}
                                className="capitalize"
                              >
                                {item}
                              </SelectItem>
                            ))}
                          </SelectGroup>
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <SheetFooter className="gap-2 pt-2 sm:space-x-0">
                <SheetClose asChild>
                  <Button type="button" variant="outline">
                    Cancel
                  </Button>
                </SheetClose>
                <Button disabled={isUpdatePending}>
                  {isUpdatePending && (
                    <ReloadIcon
                      className="mr-2 size-4 animate-spin"
                      aria-hidden="true"
                    />
                  )}
                  Save
                </Button>
              </SheetFooter>
            </form>
          </Form>
        </SheetContent>
      </Sheet>
    </div>
  )
}

export default UpdateAmmoSlotSheet
