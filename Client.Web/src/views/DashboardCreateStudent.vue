<script setup lang="ts">
import { type Estudiante } from '@/interfaces/Estudiante';
import type { Seccion } from '@/interfaces/Seccion';
import { studentService } from '@/services/estudiante.service';
import { seccionService } from '@/services/seccion.service';
import ToastService from '@/utils/ToastService';
import { onMounted, ref } from 'vue';
import { RouterLink } from 'vue-router';
import { useRouter } from 'vue-router';

const currentYear = new Date().getFullYear();
const router = useRouter();
const sections = ref<Seccion[]>([]);

onMounted(async () => {
  await cargarSecciones();
});

async function cargarSecciones() {
  try {
    const response = await seccionService.getAll();
    if (response.success && response.payload) {
      sections.value = response.payload;
    } else {
      sections.value = [];
    }
  } catch (error) {
    console.error('Error cargando secciones:', error);
    ToastService.error('Error cargando las secciones');
    sections.value = [];
  }
}

const student = ref<Estudiante>({
  nombres: '',
  apellidos: '',
  cedula: '',
  lugarNacimiento: '',
  fechaNacimiento: '',
  estadoPais: '',
  genero: '',
  numHijo: 1,
  correo: '',
  año: currentYear + ' - ' + `${currentYear + 1}`,
  condicionEspecial: 'No',
  plantesProcedencia: '',
  direccionCompleta: '',
  poseeCanaima: false,
  serial: '',
  tieneBeca: false,
  enteOtorgaBeca: '',
  tutores: [
    {
      rolTutor: 1,
      nombres: '',
      apellidos: '',
      cedula: '',
      correo: '',
      profesion: '',
      direccion: '',
      nivelEstudio: '',
      telfHabitacion: '',
      telfCelular: '',
      telfFamiliar: '',
    },
    {
      rolTutor: 2,
      nombres: '',
      apellidos: '',
      cedula: '',
      correo: '',
      profesion: '',
      direccion: '',
      telfHabitacion: '',
      telfCelular: '',
      telfFamiliar: '',
    },
    {
      rolTutor: 3,
      nombres: '',
      apellidos: '',
      cedula: '',
      correo: '',
      profesion: '',
      direccion: '',
      telfHabitacion: '',
      telfCelular: '',
      telfFamiliar: '',
    },
  ],
  controlUniforme: {
    tallaCamisa: '',
    tallaPantalon: '',
    numZapato: 30,
    peso: 0,
  },
  idSeccion: 0,
  documentoEstudianteRepresentante: {
    boletaPromocion: false,
    partidaNacimiento: false,
    copiaCedula: false,
    foto: false,
    notasCertificadas: false,
    copiaCedulaRepre: false,
    fotoRepre: false,
    autorizacionRepre: false,
  },
});

const roles: Record<number, string> = {
  1: 'Padre',
  2: 'Madre',
  3: 'Representante',
};

const isSaving = ref(false);
const message = ref<string | null>(null);

function isEmail(value: string) {
  if (!value) return false;
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}
function isNumeric(value: string | number) {
  return String(value).trim() === '' ? false : /^\d+$/.test(String(value));
}
function validDate(value: string) {
  if (!value) return false;
  const d = new Date(value);
  return !Number.isNaN(d.getTime());
}

function validateStudent(): string[] {
  const errors: string[] = [];
  const s = student.value;

  if (!s.nombres || s.nombres.trim().length < 2) errors.push('Nombres: minimo 2 caracteres');
  if (!s.apellidos || s.apellidos.trim().length < 2) errors.push('Apellidos: minimo 2 caracteres');

  if (!s.cedula || !isNumeric(s.cedula) || String(s.cedula).length < 6) {
    errors.push('Cedula: debe ser numerica y tener al menos 6 digitos');
  }

  if (!s.lugarNacimiento || s.lugarNacimiento.trim().length < 2)
    errors.push('Lugar de nacimiento: obligatorio');

  if (!s.fechaNacimiento || !validDate(s.fechaNacimiento)) {
    errors.push('Fecha nacimiento: formato invalido');
  } else {
    const dob = new Date(s.fechaNacimiento);
    const now = new Date();
    if (dob.getTime() > now.getTime()) errors.push('Fecha nacimiento: no puede ser en el futuro');
    const age = now.getFullYear() - dob.getFullYear();
    if (age < 3) errors.push('Fecha nacimiento: edad menor a 3 anos');
    if (age > 120) errors.push('Fecha nacimiento: edad improbable');
  }

  if (!s.estadoPais || s.estadoPais.trim().length < 2)
    errors.push('Estado en el pais: obligatorio');
  if (!s.genero) errors.push('Genero: seleccionar uno');

  if (s.numHijo == null || Number.isNaN(Number(s.numHijo)) || Number(s.numHijo) < 0) {
    errors.push('Numero de hijos: debe ser 0 o mayor');
  }

  if (!s.correo || !isEmail(s.correo)) errors.push('Correo: formato invalido');

  if (!s.direccionCompleta || s.direccionCompleta.trim().length < 5)
    errors.push('Direccion completa: minimo 5 caracteres');

  if (!s.año || Number(s.año) < 1900) {
    errors.push('Ano escolar: formato AAAA valido');
  }

  const tallaFields = ['tallaCamisa', 'tallaPantalon'] as const;

  tallaFields.forEach((field) => {
    const val = String(s.controlUniforme?.[field] ?? '').trim();

    if (val === '') {
      errors.push(`${field}: no puede estar vacio`);
    }
  });

  if (s.idSeccion == null || Number.isNaN(Number(s.idSeccion)))
    errors.push('Seccion: seleccionar una opcion valida');

  const numZap = Number(s.controlUniforme?.numZapato ?? 0);
  if (Number.isNaN(numZap) || numZap < 10 || numZap > 60)
    errors.push('Talla zapato: debe estar entre 10 y 60');

  const peso = Number(s.controlUniforme?.peso ?? -1);
  if (Number.isNaN(peso) || peso <= 0 || peso > 300) errors.push('Peso: valor invalido');

  if (!s.tutores || s.tutores.length === 0) {
    errors.push('Tutores: debe haber al menos un tutor');
  } else {
    s.tutores.forEach((tutor, idx) => {
      const base = `Tutor ${idx + 1}`;
      if (!tutor.nombres || tutor.nombres.trim().length < 2)
        errors.push(`${base} nombres: minimo 2 caracteres`);
      if (!tutor.apellidos || tutor.apellidos.trim().length < 2)
        errors.push(`${base} apellidos: minimo 2 caracteres`);
      if (!tutor.cedula || !isNumeric(tutor.cedula) || String(tutor.cedula).length < 6) {
        errors.push(`${base} cedula: debe ser numerica y tener al menos 6 digitos`);
      }
      if (tutor.correo && tutor.correo.trim().length > 0 && !isEmail(tutor.correo))
        errors.push(`${base} correo: formato invalido`);

      // telefonos opcionales pero si vienen validar
      const phoneFields = ['telfCelular', 'telfHabitacion', 'telfFamiliar'] as const;
      phoneFields.forEach((field) => {
        const val = tutor[field];
        if (val?.trim() === '') return;
        if (val && String(val).trim().length > 0 && !/^\d{6,20}$/.test(String(val))) {
          errors.push(`${base} ${field}: numero invalido`);
        }
      });
    });
  }

  return errors;
}

async function saveStudent() {
  // valida primero y muestra los errores via toast
  const errors = validateStudent();
  if (errors.length > 0) {
    // muestra cada error con toast para que el usuario lo vea como pides
    errors.forEach((e) => ToastService.error(e));
    message.value = errors.join(' | ');
    return;
  }

  isSaving.value = true;
  message.value = null;

  try {
    const response = await studentService.create(student.value);

    if (response.success) {
      message.value = 'Estudiante registrado correctamente';
      ToastService.success('Se ha creado el estudiante exitosamente');

      router.push('/dashboard/estudiantes');

      // limpia el formulario
      Object.assign(student.value, {
        nombres: '',
        apellidos: '',
        cedula: '',
        lugarNacimiento: '',
        fechaNacimiento: '',
        estadoPais: '',
        genero: '',
        numHijo: 1,
        año: currentYear + ' - ' + `${currentYear + 1}`,
        correo: '',
        condicionEspecial: 'No',
        plantesProcedencia: '',
        direccionCompleta: '',
        poseeCanaima: false,
        serial: '',
        tieneBeca: false,
        enteOtorgaBeca: '',
        tutores: [
          {
            rolTutor: 1,
            nombres: '',
            apellidos: '',
            cedula: '',
            correo: '',
            profesion: '',
            direccion: '',
            nivelEstudio: '',
            telfHabitacion: '',
            telfCelular: '',
            telfFamiliar: '',
          },
          {
            rolTutor: 2,
            nombres: '',
            apellidos: '',
            cedula: '',
            correo: '',
            profesion: '',
            direccion: '',
            nivelEstudio: '',
            telfHabitacion: '',
            telfCelular: '',
            telfFamiliar: '',
          },
          {
            rolTutor: 3,
            nombres: '',
            apellidos: '',
            cedula: '',
            correo: '',
            profesion: '',
            direccion: '',
            nivelEstudio: '',
            telfHabitacion: '',
            telfCelular: '',
            telfFamiliar: '',
          },
        ],
        controlUniforme: {
          tallaCamisa: '',
          tallaPantalon: '',
          numZapato: 30,
          peso: 0,
        },
        idSeccion: 0,
        documentoEstudianteRepresentante: {
          boletaPromocion: false,
          partidaNacimiento: false,
          copiaCedula: false,
          foto: false,
          notasCertificadas: false,
          copiaCedulaRepre: false,
          fotoRepre: false,
          autorizacionRepre: false,
        },
      });
    } else {
      message.value = response.message || 'Error al registrar estudiante';
    }
  } catch (error) {
    console.error('Error al registrar estudiante:', error);
    ToastService.error('Error al registrar al estudiante, revise la informacion suministrada');
    message.value = 'Ocurrio un error inesperado';
  } finally {
    isSaving.value = false;
  }
}
</script>

<template>
  <main class="p-6 space-y-6 flex flex-col">
    <section class="border border-gray-200 rounded-xl bg-white shadow-sm p-6 space-y-6">
      <h2 class="text-lg font-semibold text-gray-800 mb-4">Registrar Estudiante</h2>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Nombres</span>
          <input
            v-model="student.nombres"
            placeholder="Nombres"
            required
            minlength="2"
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Apellidos</span>
          <input
            v-model="student.apellidos"
            placeholder="Apellidos"
            required
            minlength="2"
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Cédula</span>
          <input
            v-model="student.cedula"
            placeholder="Cédula"
            required
            pattern="\\d{6,}"
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Lugar de Nacimiento</span>
          <input
            v-model="student.lugarNacimiento"
            placeholder="Lugar de Nacimiento"
            required
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Fecha de Nacimiento</span>
          <input
            v-model="student.fechaNacimiento"
            type="date"
            placeholder="Fecha de nacimiento"
            required
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Estado en el País</span>
          <input
            v-model="student.estadoPais"
            placeholder="Estado en el pais"
            type="text"
            required
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Género</span>
          <select
            v-model="student.genero"
            required
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          >
            <option disabled value="">Seleccione genero</option>
            <option value="M">Masculino</option>
            <option value="F">Femenino</option>
          </select>
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Número de Hijos</span>
          <input
            v-model="student.numHijo"
            placeholder="Número de hijos"
            type="number"
            min="0"
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Correo Electrónico</span>
          <input
            v-model="student.correo"
            placeholder="Correo Electrónico"
            type="email"
            required
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Condición Especial</span>
          <select
            v-model="student.condicionEspecial"
            required
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          >
            <option value="No">No</option>
            <option value="Si">Si</option>
          </select>
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Plantel de Procedencia</span>
          <input
            v-model="student.plantesProcedencia"
            placeholder="Plantes de procedencia"
            type="text"
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Dirección Completa</span>
          <input
            v-model="student.direccionCompleta"
            placeholder="Dirección Completa"
            required
            minlength="5"
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Año escolar</span>
          <input
            v-model="student.año"
            placeholder="Año escolar"
            type="text"
            required
            pattern="\\d{4}"
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          />
        </label>

        <label>
          <span class="block text-sm font-medium text-gray-700 mb-1">Año/Sección</span>
          <select
            v-model="student.idSeccion"
            required
            class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
          >
            <option v-for="seccion in sections" :key="seccion.idSeccion" :value="seccion.idSeccion">
              {{ seccion.descripcion }} ({{ seccion.inscripciones.length }} inscripciones)
            </option>
          </select>
        </label>
      </div>

      <!-- Tutores -->
      <div class="space-y-4">
        <h3 class="text-md font-semibold text-gray-700">Informacion de Tutores</h3>
        <div v-for="(tutor, index) in student.tutores" :key="index" class="rounded-lg p-4">
          <h4 class="font-semibold text-blue-700 mb-3">
            {{ roles[tutor.rolTutor] || 'Sin rol' }}
          </h4>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Nombres</span>
              <input
                v-model="tutor.nombres"
                placeholder="Nombres"
                required
                minlength="2"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Apellidos</span>
              <input
                v-model="tutor.apellidos"
                placeholder="Apellidos"
                required
                minlength="2"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Cédula</span>
              <input
                v-model="tutor.cedula"
                placeholder="Cédula"
                required
                pattern="\\d{6,}"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Correo</span>
              <input
                v-model="tutor.correo"
                placeholder="Correo"
                type="email"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Dirección</span>
              <input
                v-model="tutor.direccion"
                placeholder="Dirección"
                required
                minlength="5"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Teléfono Celular</span>
              <input
                v-model="tutor.telfCelular"
                placeholder="Teléfono celular"
                pattern="\\d{6,15}"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Teléfono Familiar</span>
              <input
                v-model="tutor.telfFamiliar"
                placeholder="Teléfono familiar"
                pattern="\\d{6,15}"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Teléfono Habitación</span>
              <input
                v-model="tutor.telfHabitacion"
                placeholder="Teléfono habitación"
                pattern="\\d{6,15}"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Profesión</span>
              <input
                v-model="tutor.profesion"
                placeholder="Profesión"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Nivel de Estudio</span>
              <select
                v-model="tutor.nivelEstudio"
                required
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              >
                <option disabled value="">Seleccione la el nivel de estudio</option>
                <option value="Primaria">Primaria</option>
                <option value="Secundaria">Secundaria</option>
                <option value="Bachillerato">Bachillerato</option>
                <option value="Tecnico">Tecnico</option>
                <option value="Tecnico superior">Tecnico superior</option>
                <option value="Profesional">Profesional</option>
                <option value="Maestria">Maestria</option>
                <option value="Doctorado">Doctorado</option>
              </select>
            </label>
          </div>
        </div>
      </div>

      <!-- Gestion de uniformes -->
      <div class="space-y-4">
        <h3 class="text-md font-semibold text-gray-700">Gestion de Uniformes</h3>
        <div class="rounded-lg p-4">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Talla de la camisa</span>
              <input
                v-model="student.controlUniforme.tallaCamisa"
                placeholder="Talla camisa"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Talla del pantalón</span>
              <input
                v-model="student.controlUniforme.tallaPantalon"
                placeholder="Talla pantalón"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Talla de zapatos</span>
              <input
                v-model="student.controlUniforme.numZapato"
                placeholder="Talla zapatos"
                type="number"
                min="10"
                max="60"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Peso (KG)</span>
              <input
                v-model="student.controlUniforme.peso"
                placeholder="Peso (KG)"
                type="number"
                min="1"
                max="300"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>
          </div>
        </div>
      </div>

      <!-- Documentos del estudiante -->
      <div class="space-y-4">
        <h3 class="text-md font-semibold text-gray-700">Documentos del estudiante</h3>
        <div class="rounded-lg p-4">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1"
                >Cuenta con boleta de promocion</span
              >
              <input
                v-model="student.documentoEstudianteRepresentante.boletaPromocion"
                type="checkbox"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1"
                >Cuenta con partida de nacimiento</span
              >
              <input
                v-model="student.documentoEstudianteRepresentante.partidaNacimiento"
                type="checkbox"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1"
                >Cuenta con copia de la cedula</span
              >
              <input
                v-model="student.documentoEstudianteRepresentante.copiaCedula"
                type="checkbox"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Cuenta con foto</span>
              <input
                v-model="student.documentoEstudianteRepresentante.foto"
                type="checkbox"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1"
                >Cuenta con copia de la cedula del representante</span
              >
              <input
                v-model="student.documentoEstudianteRepresentante.copiaCedulaRepre"
                type="checkbox"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1"
                >Cuenta con foto del representante</span
              >
              <input
                v-model="student.documentoEstudianteRepresentante.fotoRepre"
                type="checkbox"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1"
                >Cuenta con autorizacion en caso de no ser representante legal</span
              >
              <input
                v-model="student.documentoEstudianteRepresentante.autorizacionRepre"
                type="checkbox"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>
          </div>
        </div>
      </div>

      <!-- Información adicional -->
      <div class="space-y-4">
        <h3 class="text-md font-semibold text-gray-700">Información adicional</h3>
        <div class="rounded-lg p-4">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Posee canaima</span>
              <input
                v-model="student.poseeCanaima"
                type="checkbox"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Serial</span>
              <input
                v-model="student.serial"
                type="text"
                placeholder="Serial de la canaima (dejar vacio si no aplica)"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Tiene beca</span>
              <input
                v-model="student.tieneBeca"
                type="checkbox"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>

            <label>
              <span class="block text-sm font-medium text-gray-700 mb-1">Ente que lo otorga</span>
              <input
                v-model="student.enteOtorgaBeca"
                type="text"
                placeholder="Ente que otorga la beca(dejar vacio si no aplica)"
                class="input focus:outline-none focus:border-gray-400 focus:ring-2 rounded focus:ring-gray-400"
              />
            </label>
          </div>
        </div>
      </div>

      <div class="flex justify-end gap-3 pt-4 border-t border-gray-200">
        <RouterLink
          to="/dashboard/estudiantes"
          class="bg-gray-200 hover:bg-gray-300 text-gray-700 py-2 px-4 rounded-lg"
        >
          Cancelar
        </RouterLink>
        <button
          class="bg-blue-700 hover:bg-blue-800 text-white py-2 px-4 rounded-lg"
          @click="saveStudent"
        >
          Guardar Estudiante
        </button>
      </div>
    </section>
  </main>
</template>
