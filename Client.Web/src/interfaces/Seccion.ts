import type { Inscripcion } from './Inscripcion';

export interface Seccion {
  idSeccion?: number;
  descripcion?: string;
  inscripciones: Inscripcion[];
}
