import {Form, FormControl, FormField, FormItem, FormLabel, FormMessage} from '@/components/ui/form';
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
  SheetTrigger
} from '@/components/ui/sheet';
import React, {useEffect} from 'react';
import {useForm} from 'react-hook-form';
import {createAmmoSlotSchema, CreateAmmoSlotSchema} from "@/pages/units/customize/libs/validations";
import {createUnitAmmoSlot} from "@/api/wrapper/stats-api";
import {toast} from '@/components/ui/use-toast';
import {zodResolver} from "@hookform/resolvers/zod";
import {Button} from '@/components/ui/button';
import {ReloadIcon} from '@radix-ui/react-icons';
import {Select, SelectContent, SelectGroup, SelectItem, SelectTrigger, SelectValue} from '@/components/ui/select';
import {AmmoSlotContext, AmmoSlotContextType} from "@/pages/units/customize/information/tabs/stats/ammo-slots";

interface CreateAmmoSlotSheetProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  index: number
  unitId: number
  ammoOptions: number[]
}

const CreateAmmoSlotSheet = ({children, index, unitId, ammoOptions, ...props}: CreateAmmoSlotSheetProps) => {
  const {ammoSlots, setAmmoSlots} = React.useContext<AmmoSlotContextType>(AmmoSlotContext)
  const ammoSlot = ammoSlots[index];
  const [isCreatePending, startCreateTransition] = React.useTransition()

  const form = useForm<CreateAmmoSlotSchema>({
    resolver: zodResolver(createAmmoSlotSchema),
    defaultValues: {
      ammoHash: undefined,
      unitId: unitId,
      slotOrder: index,
    },
  })

  function onSubmit(input: CreateAmmoSlotSchema) {
    startCreateTransition(async () => {
      const id = await createUnitAmmoSlot({
        ...input,
        ammoHash: Number(input.ammoHash)
      })

      ammoSlots[index] = {
        id: id,
        ammoHash: input.ammoHash,
        slotOrder: input.slotOrder,
      }
      setAmmoSlots(ammoSlots)
      
      form.reset()
      props.onOpenChange?.(false)
      toast({
        title: `Ammo Slot ${index + 1} Created!`
      })
    })
  }

  return (
    <div>
      <Sheet {...props}>
        <SheetTrigger asChild>
          {children}
        </SheetTrigger>
        <SheetContent className="flex flex-col gap-6 sm:max-w-md">
          <SheetHeader className="text-left">
            <SheetTitle>Ammo Slot {index + 1}</SheetTitle>
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
                <Button disabled={isCreatePending}>
                  {isCreatePending && (
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
  );
};

export default CreateAmmoSlotSheet;
