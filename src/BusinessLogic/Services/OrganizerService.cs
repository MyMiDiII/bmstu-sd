﻿using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Services
{
    public interface IOrganizerService
    {
        List<Organizer> GetOrganizers();
        Organizer? GetOrganizerByID(long id);
        void CreateOrganizer(Organizer organizer);
        void UpdateOrganizer(Organizer organizer);
        void DeleteOrganizer(Organizer organizer);
        List<BoardGameEvent> GetEventsByOrganizer(Organizer organizer);
    }

    public class OrganizerService : IOrganizerService
    {
        private readonly IOrganizerRepository _organizerRepository;

        public OrganizerService(IOrganizerRepository organizerRepository)
        {
            _organizerRepository = organizerRepository;
        }

        public List<Organizer> GetOrganizers()
        {
            return _organizerRepository.GetAll();
        }

        public Organizer? GetOrganizerByID(long id)
        {
            return _organizerRepository.GetByID(id);
        }

        public void CreateOrganizer(Organizer organizer)
        {
            if (Exist(organizer))
                throw new AlreadyExistsOrganizerException();

            _organizerRepository.Add(organizer);
        }

        public void UpdateOrganizer(Organizer organizer)
        {
            if (NotExist(organizer.ID))
                throw new NotExistsOrganizerException();

            _organizerRepository.Update(organizer);
        }

        public void DeleteOrganizer(Organizer organizer)
        {
            if (NotExist(organizer.ID))
                throw new NotExistsOrganizerException();

            _organizerRepository.Delete(organizer);
        }

        private bool Exist(Organizer organizer)
        {
             return _organizerRepository.GetAll().Any(elem
                        => elem.Name == organizer.Name
                        && elem.Address == organizer.Address);
        }

        private bool NotExist(long id)
        {
            return _organizerRepository.GetByID(id) == null;
        }

        public List<BoardGameEvent> GetEventsByOrganizer(Organizer organizer)
        {
            return _organizerRepository.GetOrganizerEvents(organizer.ID);
        }
    }
}