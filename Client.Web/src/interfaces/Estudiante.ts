import type { ControlUniforme } from './ControlUniforme';
import type { Tutor } from './Tutor';
import type { DocumentoEstudianteRepresentante } from './DocumentoEstudianteRepresentante';

export interface Estudiante {
  idEstudiante?: number;
  nombres: string;
  apellidos: string;
  cedula: string;
  lugarNacimiento: string;
  fechaNacimiento: string;
  estadoPais?: string;
  genero: string;
  numHijo?: number;
  correo?: string;
  año: string;
  condicionEspecial?: string;
  plantesProcedencia?: string;
  poseeCanaima?: boolean;
  serial?: string;
  tieneBeca?: boolean
  enteOtorgaBeca?: string;
  direccionCompleta: string;
  representante?: number;
  padre?: number;
  madre?: number;
  tutores: Tutor[];
  controlUniforme: ControlUniforme;
  idSeccion: number;
  documentoEstudianteRepresentante: DocumentoEstudianteRepresentante;
}
