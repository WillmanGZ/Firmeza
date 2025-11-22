import type { ControlUniforme } from '@/interfaces/ControlUniforme';
import type { DetalleInscripcion } from '@/interfaces/DetalleInscripcion';
import type { DocumentoEstudianteRepresentante } from '@/interfaces/DocumentoEstudianteRepresentante';
import type { Estudiante } from '@/interfaces/Estudiante';
import type { Tutor } from '@/interfaces/Tutor';

export const mapDetalleToEstudiante = (
  detalle: DetalleInscripcion,
  idSeccion: number = 0, // Parámetro opcional para el ID real de la sección
): Estudiante => {
  // 1. Mapeo de Tutores (Padre, Madre, Representante)
  const tutores: Tutor[] = []; // 1.1 Padre (Rol 1)

  if (detalle.cedulaPadre || detalle.nombrePadre) {
    tutores.push({
      rolTutor: 1,
      nombres: detalle.nombrePadre || '',
      apellidos: detalle.apellidoPadre || '',
      cedula: detalle.cedulaPadre || '',
      nivelEstudio: detalle.nivelEstudioPadre,
      profesion: detalle.profesionPadre,
      direccion: detalle.direccionPadre,
      telfHabitacion: detalle.telfHabitacionPadre,
      telfCelular: detalle.telfCelularPadre,
      telfFamiliar: detalle.telfFamiliarPadre,
    });
  } // 1.2 Madre (Rol 2)

  if (detalle.cedulaMadre || detalle.nombreMadre) {
    tutores.push({
      rolTutor: 2,
      nombres: detalle.nombreMadre || '',
      apellidos: detalle.apellidoMadre || '',
      cedula: detalle.cedulaMadre || '',
      nivelEstudio: detalle.nivelEstudioMadre,
      profesion: detalle.profesionMadre,
      direccion: detalle.direccionMadre,
      telfHabitacion: detalle.telfHabitacionMadre,
      telfCelular: detalle.telfCelularMadre,
      telfFamiliar: detalle.telfFamiliarMadre,
    });
  } // 1.3 Representante (Rol 3)

  tutores.push({
    rolTutor: 3,
    nombres: detalle.nombreRepresentante,
    apellidos: detalle.apellidoRepresentante,
    cedula: detalle.cedulaRepresentante,
    nivelEstudio: detalle.nivelEstudioRepresentante,
    profesion: detalle.profesionRepresentante,
    direccion: detalle.direccionRepresentante,
    telfHabitacion: detalle.telfHabitacionRepresentante,
    telfCelular: detalle.telfCelularRepresentante,
    telfFamiliar: detalle.telfFamiliarRepresentante,
  }); // 2. Mapeo de Documentos

  const documentoEstudianteRepresentante: DocumentoEstudianteRepresentante = {
    boletaPromocion: detalle.boletaPromocion,
    partidaNacimiento: detalle.partidaNacimiento === true, // Asegura valor booleano
    copiaCedula: detalle.copiaCedula,
    foto: detalle.foto,
    notasCertificadas: detalle.notasCertificadas,
    copiaCedulaRepre: detalle.copiaCedulaRepre,
    fotoRepre: detalle.fotoRepre,
    autorizacionRepre: detalle.autorizacionRepre,
  }; // 3. Mapeo de ControlUniforme

  const controlUniforme: ControlUniforme = {
    tallaCamisa: detalle.tallaCamisa,
    tallaPantalon: detalle.tallaPantalon,
    numZapato: detalle.numZapato,
    peso: detalle.peso,
  }; // 4. Construcción del objeto Estudiante final

  const estudiante: Estudiante = {
    nombres: detalle.nombresEstudiante,
    apellidos: detalle.apellidosEstudiante,
    cedula: detalle.cedulaEstudiante,
    lugarNacimiento: detalle.lugarNacimientoEstudiante,
    fechaNacimiento: detalle.fechaNacimientoEstudiante.toString(),
    estadoPais: detalle.estadoPaisEstudiante,
    genero: detalle.generoEstudiante,
    numHijo: detalle.numHijo,
    correo: detalle.correoEstudiante,
    año: detalle.seccion.toString(), // Mapeando la sección string a 'año'
    condicionEspecial: detalle.condicionEspecialEstudiante,
    plantesProcedencia: detalle.plantelProcedenciaEstudiante,
    poseeCanaima: detalle.poseeCanaima,
    serial: detalle.serialCanaima,
    tieneBeca: detalle.tieneBeca,
    enteOtorgaBeca: detalle.enteOtorgaBeca,
    direccionCompleta: detalle.direccionEstudiante, // IDs de Tutores (pueden dejarse indefinidos si no se usan al crear/editar)
    tutores: tutores,
    controlUniforme: controlUniforme,
    idSeccion: idSeccion, // Usamos el valor pasado por parámetro (o 0 por defecto)
    documentoEstudianteRepresentante: documentoEstudianteRepresentante,
  };

  return estudiante;
};
