import { MedicamentUnit } from "../../enums/medicament-unit";

export const MEDICAMENT_UNIT_MAP: { [key in MedicamentUnit]: string } = {
  [MedicamentUnit.Milliliter]: "Mililitr",
  [MedicamentUnit.Gram]: "Gram",
  [MedicamentUnit.Capsule]: "Kapsułka",
  [MedicamentUnit.Patch]: "Plaster",
  [MedicamentUnit.Ampoule]: "Ampułka",
  [MedicamentUnit.Vial]: "Fiolka",
  [MedicamentUnit.Sachet]: "Saszetka",
  [MedicamentUnit.Drop]: "Kropla",
  [MedicamentUnit.Package]: "Opakowanie",
  [MedicamentUnit.Bottle]: "Butelka",
  [MedicamentUnit.Tube]: "Tuba",
  [MedicamentUnit.Tablet]: "Tabletka",
  [MedicamentUnit.Suppository]: "Czopek"
};
