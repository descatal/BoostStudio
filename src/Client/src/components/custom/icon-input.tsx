import * as React from "react";
import { IconProps } from "@radix-ui/react-icons/dist/types";
import { IconType } from "react-icons";

import { cn } from "@/lib/utils";

export interface InputProps
  extends React.InputHTMLAttributes<HTMLInputElement> {
  readonly?: boolean;
  startIcon?:
    | IconType
    | React.ForwardRefExoticComponent<
        IconProps & React.RefAttributes<SVGSVGElement>
      >;
  endIcon?:
    | IconType
    | React.ForwardRefExoticComponent<
        IconProps & React.RefAttributes<SVGSVGElement>
      >;
}

const IconInput = React.forwardRef<HTMLInputElement, InputProps>(
  ({ className, type, startIcon, endIcon, readonly, ...props }, ref) => {
    const StartIcon = startIcon;
    const EndIcon = endIcon;
    const hasValue = !(props?.value === undefined || props.value === "");

    return (
      <div className="relative w-full">
        {StartIcon && (
          <div className="absolute left-3 top-1/2 -translate-y-1/2 transform">
            <StartIcon size={18} className="text-muted-foreground" />
          </div>
        )}
        {!readonly ? (
          <input
            type={type}
            className={cn(
              "flex h-10 w-full rounded-md border border-input bg-background px-4 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring focus-visible:ring-offset-0 disabled:cursor-not-allowed disabled:opacity-50",
              startIcon ? "pl-10" : "",
              endIcon ? "pr-10" : "",
              className,
            )}
            ref={ref}
            {...props}
          />
        ) : (
          <span
            className={cn(
              "flex h-10 w-full items-center rounded-md border bg-background px-4 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring focus-visible:ring-offset-0 disabled:cursor-not-allowed disabled:opacity-50",
              !hasValue && "opacity-50",
              startIcon && "pl-10",
              endIcon && "pr-10",
              className,
            )}
          >
            {!hasValue ? props.placeholder : props.value}
          </span>
        )}
        {EndIcon && (
          <div className="absolute right-3 top-1/2 -translate-y-1/2 transform">
            <EndIcon className="text-muted-foreground" size={18} />
          </div>
        )}
      </div>
    );
  },
);
IconInput.displayName = "IconInput";

export { IconInput };
