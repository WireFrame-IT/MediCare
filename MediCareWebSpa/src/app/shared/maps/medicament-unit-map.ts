import { MedicamentUnit } from "../../enums/medicament-unit";

export const MEDICAMENT_UNIT_MAP: { [key in MedicamentUnit]: string } = {
  [MedicamentUnit.Ampoule]: "Ampułka",
  [MedicamentUnit.Bottle]: "Butelka",
  [MedicamentUnit.Capsule]: "Kapsułka",
  [MedicamentUnit.Suppository]: "Czopek",
  [MedicamentUnit.Vial]: "Fiolka",
  [MedicamentUnit.Gram]: "Gram",
  [MedicamentUnit.Drop]: "Kropla",
  [MedicamentUnit.Milliliter]: "Mililitr",
  [MedicamentUnit.Package]: "Opakowanie",
  [MedicamentUnit.Patch]: "Plaster",
  [MedicamentUnit.Sachet]: "Saszetka",
  [MedicamentUnit.Tablet]: "Tabletka",
  [MedicamentUnit.Tube]: "Tuba"
};
