declare module "react-big-calendar" {
    import { ComponentType } from "react";
  
    export const Calendar: ComponentType<any>;
    export const momentLocalizer: (moment: any) => any;
  }